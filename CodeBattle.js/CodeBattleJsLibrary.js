class GameClient {

    constructor(server, passwordHash, botName) {
        this.path = `ws://${server}/galaxy?token=${passwordHash}`;
        this.botName = botName;
    }

    run(callback) {
        this.socket = new WebSocket(this.path);
        this.socket.onopen = this.onOpen;
        this.socket.onerror = this.onError;
        this.socket.onclose = this.onClose;
        this.socket.onmessage = (event) => {
           this.actions = [];
           this.planets = [];
           let data = JSON.parse(event.data); 
           this.planets = data.planets;
            if (data.errors) {
                this.onError(data.errors);
            }
            callback();
            this.sendData();
        }
    }

    onOpen() {
        text.value += "Connection established\n";
    }

    onClose(event) {
        if (event.wasClean) {
            text.value += "### disconnected ###\n"
        } else {
            text.value += "### accidentally disconnected ###\n";
            text.value += " - Err code: " + event.code + ", Reason: " + event.reason + "\n";
        }
    }

    onError(error) {
        text.value += "### error ###\n" + error + "\n";
    }

    sendData() {
        const msg = {
            "actions": this.actions
        }
        text.value += "Sending: " + (msg ? JSON.stringify(msg) : 'stop') + '\n'
        this.socket.send(JSON.stringify(msg))
    }

    sendDrones(from, to, unitsCount) {
        this.actions.push({
            "from": from,
            "to": to,
            "unitsCount": unitsCount
        })
    }

    getPlanetById(id) {
        return this.planets.find(planet => {
            return planet.id && planet.id === id;
        })
    }
    
    getMyPlanets() {
        const result = [];
        this.planets.forEach(planet => {
            if (planet.owner && planet.owner === this.botName) {
                result.push(planet);
            }
        });
        return result;
    }

    getNeighbours(id) {
        const result = [];
        this.planets.find(planet => {
            if (planet.id && planet.id === id) {
                if (planet.neighbours && planet.neighbours.length > 0) {
                    planet.neighbours.forEach(neighbour => {
                        result.push(this.getPlanetById(neighbour));
                    })
                }
            };
        })
        return result;
    }
    
}



