package clientlib;

import clientlib.domain.server.GalaxySnapshot;
import lombok.extern.slf4j.Slf4j;

import java.net.URISyntaxException;

@Slf4j
public class CodeBattleJava {

    public static void main(String[] args) throws URISyntaxException {
        CodeBattleJavaClient client = new CodeBattleJavaClient("localhost:8080", "<token>", "<login>");
        client.run((cl) -> {
            GalaxySnapshot snapshot = cl.getGalaxy();
            if (!snapshot.getErrors().isEmpty()) {
                log.error("Error occurred", snapshot.getErrors());
            }

            cl.getMyPlanets().forEach(planet -> {
                planet.getNeighbours().stream()
                        .filter(neighbourId -> neighbourId != planet.getId())
                        .forEach(neighbourId -> cl.sendDrones(planet.getId(), neighbourId, planet.getDroids() / planet.getNeighbours().size()));
            });
        });
    }
}
