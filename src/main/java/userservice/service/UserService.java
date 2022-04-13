package userservice.service;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.security.core.userdetails.UserDetailsService;
import userservice.model.Role;
import userservice.model.User;

public interface UserService extends UserDetailsService {
    User saveUser(User user);
    Role saveRole(Role role);
    void addRoleToUser(String username, String roleName);
    User getUser(String username);
    Page<User> getUsers(Pageable pageable);
    Page<Role> getRoles(Pageable pageable);
}
