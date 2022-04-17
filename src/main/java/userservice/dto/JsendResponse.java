package userservice.dto;

import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
public class JsendResponse<T> {

    public static final String SUCCESS = "success";
    public static final String FAIL = "fail";
    public static final String ERROR = "error";

    private String status;
    private T data;
    private String link;

    private JsendResponse(String status, T data) {
        this.status = status;
        this.data = data;
        this.link = "https://stackoverflow.com/";
    }
    private JsendResponse(String status) {
        this.status = status;
        this.data = null;
        this.link = "https://stackoverflow.com/";
    }

    public static <T> JsendResponse<T> success(T data) {
        return new JsendResponse<>(SUCCESS, data);
    }
    public static JsendResponse<?> emptySuccess() {
        return new JsendResponse<>();
    }

    public static <T> JsendResponse<T> fail(T data) {
        return new JsendResponse<>(FAIL, data);
    }

    public static <T> JsendResponse<T> error(T data) {
        return new JsendResponse<>(ERROR, data);
    }
}
