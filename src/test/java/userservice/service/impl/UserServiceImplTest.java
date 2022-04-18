package userservice.service.impl;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.test.context.junit4.SpringRunner;
import userservice.dto.RegisterRequest;
import userservice.model.Role;
import userservice.model.User;
import userservice.repository.RoleRepository;
import userservice.repository.UserRepository;

import javax.validation.ConstraintViolation;
import javax.validation.Validation;
import javax.validation.Validator;
import javax.validation.ValidatorFactory;
import java.util.*;

import static org.junit.Assert.assertEquals;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.BDDMockito.given;


@RunWith(SpringRunner.class)
public class UserServiceImplTest {

    @InjectMocks
    private UserServiceImpl userService;
    @Mock
    private UserRepository userRepository;
    @Mock
    private RoleRepository roleRepository;
    @Mock
    private PasswordEncoder passwordEncoder;

    @Test
    public void registerUserCorrectlyTest() {
        //Given
        User newUser = new User("name", "uniqueUsername", "password");
        Role userRole = new Role(1L, Role.ROLE_USER);
        User expectedResult = new User(1L, "name", "uniqueUsername", "encodedPassword", new ArrayList<>(List.of(userRole)));
        given(roleRepository.findByName(Role.ROLE_USER)).willReturn(Optional.of(userRole));
        given(userRepository.saveAndFlush(any(User.class))).willReturn(newUser.setId(1L));
        given(passwordEncoder.encode(any(String.class))).willReturn("encodedPassword");
        //When
        userService = new UserServiceImpl(userRepository, roleRepository, passwordEncoder);
        User result = userService.saveUser(newUser);
        //Then
        assertEquals(result, expectedResult);
    }

    @Test
    public void registerUserValidateInvalidRegisterRequestFields() {
        //Given
        RegisterRequest request = new RegisterRequest("", "", "");
        ValidatorFactory factory = Validation.buildDefaultValidatorFactory();
        Validator validator = factory.getValidator();
        //When
        Set<ConstraintViolation<RegisterRequest>> constraintViolations = validator.validate(request);
        //Then
        assertEquals(3, constraintViolations.size());
    }

}
