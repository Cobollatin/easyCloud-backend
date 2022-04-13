package userservice;

import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.data.domain.Pageable;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import userservice.model.Role;
import userservice.model.User;
import userservice.service.UserService;
import userservice.util.JwtCenter;

import java.util.ArrayList;

@SpringBootApplication
public class UserServiceApplication {

    public static void main(String[] args) {
        SpringApplication.run(UserServiceApplication.class, args);
    }
    @Bean
    public JwtCenter getJwtCenter() {
        return new JwtCenter();
    }
    @Bean
    PasswordEncoder passwordEncoder(){
        return new BCryptPasswordEncoder();
    }

    @Bean
    CommandLineRunner run(UserService userService){
        return args -> {
            if (userService.getUsers(Pageable.ofSize(10)).isEmpty()){
                userService.saveRole(new Role(null, "ROLE_USER"));
                userService.saveRole(new Role(null, "ROLE_MANAGER"));
                userService.saveRole(new Role(null, "ROLE_ADMIN"));
                userService.saveRole(new Role(null, "ROLE_SUPER_ADMIN"));
                userService.saveUser(new User(null, "Diego Johnson", "diego", "1234", new ArrayList<>()));
                userService.saveUser(new User(null, "Ashlyn Demrest", "ashlyn", "1234", new ArrayList<>()));
                userService.saveUser(new User(null, "Anibal Ludena", "anibal", "1234", new ArrayList<>()));
                userService.saveUser(new User(null, "Jim Carry", "jim", "1234", new ArrayList<>()));

                userService.addRoleToUser("diego", "ROLE_USER");
                userService.addRoleToUser("diego", "ROLE_MANAGER");
                userService.addRoleToUser("diego", "ROLE_ADMIN");
                userService.addRoleToUser("diego", "ROLE_SUPER_ADMIN");
                userService.addRoleToUser("ashlyn", "ROLE_USER");
                userService.addRoleToUser("ashlyn", "ROLE_SUPER_ADMIN");
                userService.addRoleToUser("anibal", "ROLE_USER");
                userService.addRoleToUser("jim", "ROLE_USER");
            }
        };
    }

}
