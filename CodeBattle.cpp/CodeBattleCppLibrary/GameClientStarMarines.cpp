#include "GameClientStarMarines.h"
#include "rapidjson/document.h"
#include "rapidjson/stringbuffer.h"
#include "rapidjson/document.h"
#include "easywsclient/easywsclient.hpp"
#include <iostream>

using namespace rapidjson;

GameClientStarMarines::GameClientStarMarines(std::string _server, std::string player, std::string token)
{
	snapshot = nullptr;
	this->player = player;
	path = "ws://" + _server + "/galaxy" + "?token=" + token;
	is_running = false;
}

GameClientStarMarines::~GameClientStarMarines()
{
	is_running = false;
	snapshot = nullptr;
	work_thread->join();
}

void GameClientStarMarines::Run(std::function<void()> _message_handler)
{
	is_running = true;
	work_thread = new std::thread(&GameClientStarMarines::update_func, this, _message_handler);
}

void GameClientStarMarines::update_func(std::function<void()> _message_handler)
{
#ifdef _WIN32
	WSADATA wsaData;

	if (WSAStartup(MAKEWORD(2, 2), &wsaData))
		throw new std::exception("WSAStartup Failed.\n");
	else
		std::cout << "Connection established" << std::endl;
#endif

	web_socket = easywsclient::WebSocket::from_url(path);
	if (web_socket == nullptr)is_running = false;
	while (is_running)
	{
		web_socket->poll();
		auto state = web_socket->getReadyState();
		if (state != easywsclient::WebSocket::OPEN && state != easywsclient::WebSocket::CONNECTING)
		{
			std::cout << "Error: closing socket." << std::endl;
			is_running = false;
			break;
		}
		web_socket->dispatch([&](const std::string &message)
		{
			const char* cstr = message.c_str();
			Document d;
			d.Parse(cstr);
			Value& planets = d["planets"];
			Value& disasters = d["disasters"];
			Value& portals = d["portals"];
			Value& errors = d["errors"];
			std::list<DisasterInfo> disastersInfo;
			std::list<PlanetInfo> planetsInfo;
			std::list<Portal> portalsInfo;
			std::list<std::string> errorsInfo;
			if (planets.IsArray()) {
				for (auto& planet : planets.GetArray()) {
					PlanetInfo planetInfo;
					planetInfo.setId(planet["id"].GetInt());
					planetInfo.setDroids(planet["droids"].GetInt());
					if (planet["owner"].IsString()) {
						planetInfo.setOwner(planet["owner"].GetString());
					}
					if (planet["type"].IsString()) {
						planetInfo.setType(planet["type"].GetString());
					}
					if (planet["neighbours"].IsArray()) {
						std::list<int> neighbours;
						for (auto& neighbour : planet["neighbours"].GetArray()) {
							neighbours.push_back(neighbour.GetInt());
						}
						planetInfo.setNeighbours(neighbours);
					}
					planetsInfo.push_back(planetInfo);
				}
				for (auto& disaster : disasters.GetArray()) {
					std::string disasterType;
					DisasterInfo disasterInfo;
					if(disaster["type"].IsString()) {
						disasterType = std::string(disaster["type"].GetString());
						disasterInfo.setType(disasterType);
					}
					if (disasterType.compare("METEOR") == 0) {
						disasterInfo.setPlanetId(disaster["planetId"].GetInt());
					}
					else {
						disasterInfo.setSourcePlanetId(disaster["sourcePlanetId"].GetInt());
						disasterInfo.setTargetPlanetId(disaster["targetPlanetId"].GetInt());
						disastersInfo.push_back(disasterInfo);
					}
				}
				for (auto& portal : portals.GetArray()) {
					Portal portalInfo;
					portalInfo.setSource(portal["source"].GetInt());
					portalInfo.setTarget(portal["target"].GetInt());
					portalsInfo.push_back(portalInfo);
				}
				for (auto& error : errors.GetArray()) {
					if (error.IsString()) {
						errorsInfo.push_back(error.GetString());
					}
				}
				snapshot = new GalaxySnapshot(disastersInfo, planetsInfo, portalsInfo, errorsInfo);
			}
			_message_handler();
			actions.clear();
		});
	}
	if (web_socket)web_socket->close();

#ifdef _WIN32
	WSACleanup();
#endif
}
