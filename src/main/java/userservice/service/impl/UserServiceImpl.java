package userservice.service.impl;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.web.client.HttpClientErrorException;
import userservice.model.Role;
import userservice.model.User;
import userservice.repository.RoleRepository;
import userservice.repository.UserRepository;
import userservice.service.UserService;

import javax.transaction.Transactional;
import java.util.ArrayList;
import java.util.Collection;

@Service @RequiredArgsConstructor @Transactional @Slf4j
public class UserServiceImpl implements UserService {

    private final UserRepository userRepository;
    private final RoleRepository roleRepository;
    private final PasswordEncoder passwordEncoder;

    @Override
    public User saveUser(User user) {
        log.info("Saving new user {} to the database", user.getName());
        user.setPassword(passwordEncoder.encode(user.getPassword()));
        user.getRoles().add(roleRepository.findByName(Role.ROLE_USER)
                .orElseThrow(() -> new HttpClientErrorException(HttpStatus.NOT_FOUND)));
        return userRepository.saveAndFlush(user);
    }

    @Override
    public Role saveRole(Role role) {
        log.info("Saving new role {} to the database", role.getName());
        return roleRepository.saveAndFlush(role);
    }

    @Override
    public void addRoleToUser(String username, String roleName) {
        log.info("Adding role {} to user {}", roleName, username);
        User user = userRepository.findByUsername(username)
                .orElseThrow(()-> new UsernameNotFoundException("User not found"));
        Role role = roleRepository.findByName(roleName)
                .orElseThrow(() -> new HttpClientErrorException(HttpStatus.NOT_FOUND));
        user.getRoles().add(role);
    }

    @Override
    public User getUser(String username) {
        return userRepository.findByUsername(username)
                .orElseThrow(()-> new UsernameNotFoundException("User not found"));
    }
    @Override
    public Page<User> getUsers(Pageable pageable) {
        return userRepository.findAll(pageable);
    }

    @Override
    public Page<Role> getRoles(Pageable pageable) {
        return roleRepository.findAll(pageable);
    }
    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        User user = userRepository.findByUsername(username)
                .orElseThrow(() -> {
                    log.error("User not found in the database");
                    return new UsernameNotFoundException("User not found");
                });
        log.info("User found in the database");
        Collection<SimpleGrantedAuthority> authorities = new ArrayList<>();
        user.getRoles().forEach(role -> authorities.add(new SimpleGrantedAuthority(role.getName())));
        return new org.springframework.security.core.userdetails.User(user.getUsername(), user.getPassword(), authorities);

    }
}
