package clientlib.domain.server;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.ArrayList;
import java.util.List;


@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class GalaxySnapshot {
    @Builder.Default
    private List<PlanetInfo> planets = new ArrayList<>();
    @Builder.Default
    private List<DisasterInfo> disasters = new ArrayList<>();
    @Builder.Default
    private List<Edge> portals = new ArrayList<>();
    @Builder.Default
    private List<String> errors = new ArrayList<>();
}
