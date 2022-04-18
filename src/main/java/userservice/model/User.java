package userservice.model;

import lombok.AllArgsConstructor;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.Collection;

@Entity
@Table(name = "users")
@Getter
@AllArgsConstructor
@NoArgsConstructor
@EqualsAndHashCode
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String name;
    @Column(unique = true)
    private String username;
    private String password;

    @ManyToMany(fetch = FetchType.EAGER, cascade = CascadeType.ALL)
    @JoinTable(name = "users_roles",
            joinColumns = @JoinColumn(name = "user_id", referencedColumnName = "id"),
            inverseJoinColumns = @JoinColumn(name = "role_id", referencedColumnName = "id"))
    private Collection<Role> roles;

    public User(String name, String username, String password) {
        this.id = null;
        this.name = name;
        this.username = username;
        this.password = password;
        this.roles = new ArrayList<>();
    }

    public User setName(String name) {
        this.name = name;
        return this;
    }

    public User setUsername(String username) {
        this.username = username;
        return this;
    }

    public User setPassword(String password) {
        this.password = password;
        return this;
    }

    public User setRoles(Collection<Role> roles) {
        this.roles = roles;
        return this;
    }

    public User setId(Long id) {
        this.id = id;
        return this;
    }
}