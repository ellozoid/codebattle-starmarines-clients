#include <iostream>
#include <random>

#include "GameClientStarMarines.h"

void main()
{

	srand(time(0));
	GameClientStarMarines *gcb = new GameClientStarMarines("localhost:8080", "<login>", "<token>");
	gcb->Run([&]()
	{
		if (gcb->getErrors().size() != 0) {
			for (auto& error : gcb->getErrors()) {
				std::cout << error << '\n';
			}
		}
		for (auto& myPlanet : gcb->getMyPlanets()) {
			for (auto& neighbour : myPlanet.getNeighbours()) {
				if (neighbour != myPlanet.getId()) {
					gcb->sendDrones(myPlanet.getId(), neighbour, myPlanet.getDroids() / myPlanet.getNeighbours().size());
				}
			}
		}
		gcb->sendMessage();
	});

	getchar();
}
