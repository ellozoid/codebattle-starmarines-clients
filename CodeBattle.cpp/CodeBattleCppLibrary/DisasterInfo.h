#pragma once

class DisasterInfo {
private:
	std::string type;
	int sourcePlanetId;
	int targetPlanetId;
	int planetId;
public:
	void setType(std::string type) {
		this->type = type;
	}
	void setSourcePlanetId(int sourcePlanetId) {
		this->sourcePlanetId = sourcePlanetId;
	}
	void setTargetPlanetId(int targetPlanetId) {
		this->targetPlanetId = targetPlanetId;
	}
	void setPlanetId(int planetId) {
		this->planetId = planetId;
	}
	std::string getType() {
		return type;
	}
	int getSourcePlanetId() {
		return sourcePlanetId;
	}
	int getTargetPlanetId() {
		return targetPlanetId;
	}
	int getPlanetId() {
		return planetId;
	}
};