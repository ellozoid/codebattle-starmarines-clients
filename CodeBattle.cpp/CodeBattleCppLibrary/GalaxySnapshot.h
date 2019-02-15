#include <list>
#include "DisasterInfo.h"
#include "PlanetInfo.h"
#include "Portal.h"

class GalaxySnapshot {
private:
	std::list<DisasterInfo> disasters;
	std::list<PlanetInfo> planets;
	std::list<Portal> portals;
	std::list<std::string> errors;
public:
	GalaxySnapshot(std::list<DisasterInfo> disasters, std::list<PlanetInfo> planets, std::list<Portal> portals, std::list<std::string> errors) {
		this->disasters = disasters;
		this->planets = planets;
		this->portals = portals;
		this->errors = errors;
	}
	~GalaxySnapshot() {
		std::cout << "DESTRUCTOR _______________________________________________" << '\n';
		disasters.clear();
		planets.clear();
		portals.clear();
		errors.clear();
	}

	std::list<DisasterInfo> getDisasters() {
		return this->disasters;
	}
	std::list<PlanetInfo> getPlanets() {
		return this->planets;
	}
	std::list<Portal> getPortals() {
		return this->portals;
	}
	std::list<std::string> getErrors() {
		return this->errors;
	}
	void setDisasters(std::list<DisasterInfo> disasters) {
		this->disasters = disasters;
	}
	void setPlanets(std::list<PlanetInfo> planets) {
		this->planets = planets;
	}
	void setPortals(std::list<Portal> portals) {
		this->portals = portals;
	}
	void setErrors(std::list<std::string> errors) {
		this->errors = errors;
	}
};