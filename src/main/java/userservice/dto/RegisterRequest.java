package userservice.dto;

import lombok.AllArgsConstructor;
import lombok.Data;

import javax.validation.constraints.NotBlank;

@Data @AllArgsConstructor
public class RegisterRequest {
    @NotBlank
    private String name;
    @NotBlank
    private String username;
    @NotBlank
    private String password;
}
