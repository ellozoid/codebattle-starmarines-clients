package clientlib.domain.server;

import lombok.AllArgsConstructor;
import lombok.Data;


@Data
@AllArgsConstructor
public class Edge {
    private long source;
    private long target;
}
