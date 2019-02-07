import logging

from starmarinesclient.CodeBattlePythonLibrary import GameClient, ClientCommand, GalaxySnapshot, ClientAction

logging.basicConfig(format='%(asctime)s %(levelname)s:%(message)s',
                    level=logging.INFO)


def turn(cl: GameClient):
    errors = cl.get_galaxy().errors

    if errors:
        print("Error occurred", errors)
        
    annexed_planets = cl.get_my_planets()
    for annexed in annexed_planets:
        neighbours = cl.get_neighbours(annexed.id)
        for neighbour in neighbours:
            cl.send_drones(annexed.id, neighbour.id, int(annexed.droids / len(neighbours)))


def main():
    client = GameClient("localhost:8080", "<token>", "<login>")
    client.run(turn)


if __name__ == '__main__':
    main()
