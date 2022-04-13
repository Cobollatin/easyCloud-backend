package userservice.dto;

import com.fasterxml.jackson.annotation.JsonFilter;
import lombok.Builder;
import lombok.Data;
import org.springframework.http.HttpStatus;

@Data
public class AuthenticationResponse {

    private Long id;
    private String username;
    private String access_token;
    private String refresh_token;
    private String message;
    private HttpStatus httpStatus;

    public AuthenticationResponse(String access_token, String refresh_token, String username,  String message, HttpStatus httpStatus, Long id) {
        this.username = username;
        this.access_token = access_token;
        this.refresh_token = refresh_token;
        this.message = message;
        this.httpStatus = httpStatus;
        this.id = id;
    }
    public AuthenticationResponse(String access_token, String refresh_token, String username,  String message, HttpStatus httpStatus) {
        this.username = username;
        this.access_token = access_token;
        this.refresh_token = refresh_token;
        this.message = message;
        this.httpStatus = httpStatus;
    }
}
