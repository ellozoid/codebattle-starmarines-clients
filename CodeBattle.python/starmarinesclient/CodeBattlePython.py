import logging

from starmarinesclient.CodeBattlePythonLibrary import GameClient, ClientCommand, GalaxySnapshot, ClientAction

logging.basicConfig(format='%(asctime)s %(levelname)s:%(message)s',
                    level=logging.DEBUG)


def turn(cl: GameClient):
    """
    Задача игрока: реализовать логику формирования команд на отправку дронов в этом методе.
    Для отправки дронов с планеты на планету используется метод :func:`starmarinesclient.GameClient.send_drones`

    Получить снапшот галактики можно используя метол :func:`starmarinesclient.GameClient.get_galaxy`

    Получение всех аннексированных твоими дронами планет можно методом :func:`starmarinesclient.GameClient.get_my_planets`

    Получение всех соседей планеты по её илентификатору: :func:`starmarinesclient.GameClient.get_neighbours`

    Получение описания планеты по её идентификатору: :func:`starmarinesclient.GameClient.get_planet_by_id`

    :param cl: Вспомогательный объект клиента
    :return метод ничего не возвращает
    """
    errors = cl.get_galaxy().errors

    if errors:
        print("Error occurred", errors)  # выводим информацию об ошибках, если таковые есть (например, с клиента
        # отправлено невалидное действие)

    annexed_planets = cl.get_my_planets()  # получаем список своих планет
    for annexed in annexed_planets:
        neighbours = cl.get_neighbours(annexed.id) # для каждой аннексированной планеты получаем её соседей
        for neighbour in neighbours:
            cl.send_drones(annexed.id, neighbour.id, annexed.droids // len(neighbours))  # отсылаем дронов с
            # аннексированных планет на соседние планеты


def main():
    """
    Указываем адрес сервера, токен и логин игрока
    """
    client = GameClient("localhost:8080", "<token>", "<login>")
    client.run(turn)


if __name__ == '__main__':
    main()
