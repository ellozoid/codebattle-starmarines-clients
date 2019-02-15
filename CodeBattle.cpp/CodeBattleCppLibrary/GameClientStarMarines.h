#pragma once

#include <string>
#include <thread>
#include "easywsclient\easywsclient.hpp"
#ifdef _WIN32
#pragma comment( lib, "ws2_32" )
#include <WinSock2.h>
#endif
#include <assert.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <memory>

#include "GalaxySnapshot.h"
#include "ClientAction.h"

class GameClientStarMarines
{
	GalaxySnapshot* snapshot;
	std::string player;
	std::list<ClientAction> actions;

	easywsclient::WebSocket *web_socket;
	std::string path;

	bool is_running;
	std::thread *work_thread;
	void update_func(std::function<void()> _message_handler);

public:
	GameClientStarMarines(std::string _server, std::string player, std::string token = "");
	~GameClientStarMarines();

	void Run(std::function<void()> _message_handler);
	void sendDrones(int from, int to, int drones) {
		ClientAction* action = new ClientAction(from,to,drones);
		actions.push_back(*action);
	};
	std::list<std::string> getErrors() {
		return (*snapshot).getErrors();
	}

	PlanetInfo getPlanetById(int id) {
		for (auto& planet : (*snapshot).getPlanets()) {
			if (planet.getId() == id) {
				return planet;
			}
		}
	}

	std::list<PlanetInfo> getNeighbours(int planetId) {
		std::list<PlanetInfo> neighbours;
		for (auto& planet : (*snapshot).getPlanets()) {
			std::list<int> planetNeighbours = planet.getNeighbours();
			auto result = std::find(planetNeighbours.begin(), planetNeighbours.end(), planetId);
			if (result != planetNeighbours.end()) {
				neighbours.push_back(planet);
			}
		}
		return neighbours;
	}

	std::list<PlanetInfo> getMyPlanets() {
		std::list<PlanetInfo> myPlanets;
		for (auto& planet : (*snapshot).getPlanets()) {
			if (player.compare(planet.getOwner()) == 0) {
				myPlanets.push_back(planet);
			}
		}
		return myPlanets;
	}

	void Blank() {
		send(""); 
	}

	void sendMessage() {
		std::string commands = "{ \"actions\":[";
		for (auto& action : actions) {
			commands += "{\"from\":" + std::to_string(action.getSrc()) +
				", \"to\":" + std::to_string(action.getDest()) +
				", \"unitsCount\":" + std::to_string(action.getUnitCounts()) +
				"},";
		}
		commands = commands.substr(0, commands.size() - 1);
		commands += "]}";
		send(commands);
	}

	GalaxySnapshot* get_galaxy_snapshot() {
		return snapshot;
	}

private:
	void send(std::string msg)
	{
		std::cout << "Sending: " << msg << std::endl;
		web_socket->send(msg);
	}
};
