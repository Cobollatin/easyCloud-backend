package userservice;

import org.springframework.boot.test.context.TestConfiguration;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Primary;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.provisioning.InMemoryUserDetailsManager;

import java.util.ArrayList;
import java.util.List;

@TestConfiguration
public class SpringSecurityTestConfig {

    @Bean
    @Primary
    public UserDetailsService userDetailsService() {
        User basicUser = new org.springframework.security.core.userdetails.User("diego", "pass", new ArrayList<>());
        return new InMemoryUserDetailsManager(List.of(basicUser));
    }
}