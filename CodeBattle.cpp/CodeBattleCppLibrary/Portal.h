#pragma once

class Portal {
private:
	int source;
	int target;
public:

	int getSource() {
		return source;
	}

	int getTarget() {
		return target;
	}

	void setSource(int source) {
		this->source = source;
	}

	void setTarget(int target) {
		this->target = target;
	}
};