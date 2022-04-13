package userservice.util;

import com.auth0.jwt.JWT;
import com.auth0.jwt.JWTVerifier;
import com.auth0.jwt.algorithms.Algorithm;
import com.auth0.jwt.interfaces.DecodedJWT;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import userservice.model.Role;
import userservice.model.User;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.stream.Collectors;

import static java.util.Arrays.stream;

public class JwtCenter {
    public final long JWT_ACCESS_TOKEN_VALIDITY = 6 * 60 * 100L;
    public final long JWT_REFRESH_TOKEN_VALIDITY = 60 * 60 * 10000L;

    @Value("${jwt.secret}")
    private String secret;

    public String generateAccessToken(UserDetails userDetails, String requestPath){
        Algorithm algorithm = Algorithm.HMAC256(this.secret.getBytes());
        return JWT.create()
                .withSubject(userDetails.getUsername())
                .withExpiresAt(new Date(System.currentTimeMillis() + JWT_ACCESS_TOKEN_VALIDITY))
                .withIssuer(requestPath)
                .withClaim("roles", userDetails.getAuthorities().stream().map(GrantedAuthority::getAuthority).collect(Collectors.toList()))
                .sign(algorithm);
    }
    public String generateAccessToken(User user, String requestPath){
        Algorithm algorithm = Algorithm.HMAC256(this.secret.getBytes());
        return JWT.create()
                .withSubject(user.getUsername())
                .withExpiresAt(new Date(System.currentTimeMillis() + JWT_ACCESS_TOKEN_VALIDITY))
                .withIssuer(requestPath)
                .withClaim("roles", user.getRoles().stream().map(Role::getName).collect(Collectors.toList()))
                .sign(algorithm);
    }
    public String generateRefreshToken(UserDetails userDetails, String requestPath){
        Algorithm algorithm = Algorithm.HMAC256(this.secret.getBytes());
        return JWT.create()
                .withSubject(userDetails.getUsername())
                .withExpiresAt(new Date(System.currentTimeMillis() + JWT_REFRESH_TOKEN_VALIDITY))
                .withIssuer(requestPath)
                .sign(algorithm);
    }
    public String generateRefreshToken(User user, String requestPath){
        Algorithm algorithm = Algorithm.HMAC256(this.secret.getBytes());
        return JWT.create()
                .withSubject(user.getUsername())
                .withExpiresAt(new Date(System.currentTimeMillis() + JWT_REFRESH_TOKEN_VALIDITY))
                .withIssuer(requestPath)
                .sign(algorithm);
    }

    public DecodedJWT verifyAndDecodeToken(String token){
        Algorithm algorithm = Algorithm.HMAC256(this.secret.getBytes());
        JWTVerifier verifier = JWT.require(algorithm).build();
        return verifier.verify(token);
    }

    public UsernamePasswordAuthenticationToken authenticationToken(String token){
        DecodedJWT decodedJWT = this.verifyAndDecodeToken(token);
        String username = decodedJWT.getSubject();
        String[] roles = decodedJWT.getClaim("roles").asArray(String.class);
        Collection<SimpleGrantedAuthority> authorities = new ArrayList<>();
        stream(roles).forEach(role -> {
            authorities.add(new SimpleGrantedAuthority(role));
        });
        return new UsernamePasswordAuthenticationToken(username, null, authorities);
    }
}
