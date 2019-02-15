#pragma once
#include <list>

class PlanetInfo {
private:
	int id;
	int droids;
	std::string owner;
	std::string type;
	std::list<int> neighbours;
public:
	void setId(int id) {
		this->id = id;
	}
	void setDroids(int droids) {
		this->droids = droids;
	}
	void setOwner(std::string owner) {
		this->owner = owner;
	}
	void setType(std::string type) {
		this->type = type;
	}
	void setNeighbours(std::list<int> neighbours) {
		this->neighbours = neighbours;
	}
	int getId() {
		return id;
	}
	int getDroids() {
		return droids;
	}
	std::string getOwner() {
		return owner;
	}
	std::string getType() {
		return type;
	}
	std::list<int> getNeighbours() {
		return neighbours;
	}
};