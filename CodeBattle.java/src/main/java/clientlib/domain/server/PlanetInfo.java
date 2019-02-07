package clientlib.domain.server;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;


@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class PlanetInfo {
    private long id;
    private long droids;
    private String owner;
    private PlanetType type;
    private List<Long> neighbours;
}
