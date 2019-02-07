package clientlib.domain.server;

import lombok.Builder;
import lombok.Data;


@Data
@Builder
public class DisasterInfo {
    private DisasterType type;
    private Long planetId;
    private Long sourcePlanetId;
    private Long targetPlanetId;
}
