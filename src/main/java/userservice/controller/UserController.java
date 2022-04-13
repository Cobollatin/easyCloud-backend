package userservice.controller;

import com.auth0.jwt.interfaces.DecodedJWT;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;
import userservice.dto.AuthenticationRequest;
import userservice.dto.AuthenticationResponse;
import userservice.dto.RoleToUserForm;
import userservice.model.Role;
import userservice.model.User;
import userservice.service.UserService;
import userservice.util.JwtCenter;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.net.URI;
import java.util.*;

import static org.springframework.http.HttpHeaders.AUTHORIZATION;
import static org.springframework.http.HttpStatus.FORBIDDEN;
import static org.springframework.http.MediaType.APPLICATION_JSON_VALUE;


@RestController
@RequestMapping("/api")
@RequiredArgsConstructor
@Slf4j
public class UserController {

    private final JwtCenter jwtCenter;
    private final UserService userService;
    private final AuthenticationManager authenticationManager;

    @GetMapping("/users")
    public ResponseEntity<Page<User>> getUsers(Pageable pageable){
        return ResponseEntity.ok().body(userService.getUsers(pageable));
    }

    @PostMapping("/users/register")
    public ResponseEntity<User> saveUser(@RequestBody User user){
        URI uri = URI.create(ServletUriComponentsBuilder.fromCurrentContextPath().path("/api/users").toUriString());
        return ResponseEntity.created(uri).body(userService.saveUser(user));
    }

    @PostMapping("/users/login")
    public ResponseEntity<?> login(@RequestBody AuthenticationRequest loginForm, HttpServletRequest request){
        String username = loginForm.getUsername();
        String password = loginForm.getPassword();
        log.info("Username is: {}", username); log.info("Password is: {}", password);
        UsernamePasswordAuthenticationToken authenticationToken = new UsernamePasswordAuthenticationToken(username, password);

        try {
            authenticationManager.authenticate(authenticationToken);
        }
        catch (Exception exception){
            log.error("Error logging in {}", exception.getMessage());
            Map<String,String> error = new HashMap<>();
            error.put("error_message", exception.getMessage());
            return ResponseEntity.status(FORBIDDEN).body(error);
        }

        User user = userService.getUser(username);
        String access_token = jwtCenter.generateAccessToken(user, request.getContextPath());
        String refresh_token = jwtCenter.generateRefreshToken(user, request.getContextPath());
        AuthenticationResponse authenticationResponse= new AuthenticationResponse(access_token, refresh_token, user.getUsername(), "success", HttpStatus.OK, user.getId());
        return ResponseEntity.ok().body(authenticationResponse);
    }

    @GetMapping("/roles")
    public ResponseEntity<Page<Role>> getRoles(Pageable pageable){
        return ResponseEntity.ok().body(userService.getRoles(pageable));
    }

    @PostMapping("/roles")
    public ResponseEntity<Role> saveRole(@RequestBody Role role){
        URI uri = URI.create(ServletUriComponentsBuilder.fromCurrentContextPath().path("/api/roles").toUriString());
        return ResponseEntity.created(uri).body(userService.saveRole(role));
    }
    @PostMapping("/roles/addToUser")
    public ResponseEntity<?> addRoleToUser(@RequestBody RoleToUserForm form){
        userService.addRoleToUser(form.getUsername(), form.getRoleName());
        return ResponseEntity.ok().build();
    }

    @GetMapping("/token/refresh")
    public void refreshToken(HttpServletRequest request, HttpServletResponse response) throws IOException {
        String authorizationHeader = request.getHeader(AUTHORIZATION);
        if(authorizationHeader != null && authorizationHeader.startsWith("Bearer ")){
            try {
                String refresh_token = authorizationHeader.substring("Bearer ".length());
                DecodedJWT decodedJWT = jwtCenter.verifyAndDecodeToken(refresh_token);
                String username = decodedJWT.getSubject();
                User user = userService.getUser(username);
                String access_token = jwtCenter.generateAccessToken(user, request.getRequestURL().toString());
                AuthenticationResponse authenticationResponse = new AuthenticationResponse(access_token, refresh_token, username, "success", HttpStatus.OK, user.getId());
                response.setContentType(APPLICATION_JSON_VALUE);
                new ObjectMapper().writeValue(response.getOutputStream(), authenticationResponse);

            } catch (Exception exception){
                response.setHeader("error", exception.getMessage());
                response.setStatus(FORBIDDEN.value());
                Map<String,String> error = new HashMap<>();
                error.put("error_message", exception.getMessage());
                response.setContentType(APPLICATION_JSON_VALUE);
                new ObjectMapper().writeValue(response.getOutputStream(), error);
            }
        } else {
            throw new RuntimeException("Refresh token is missing");
        }
    }
}



