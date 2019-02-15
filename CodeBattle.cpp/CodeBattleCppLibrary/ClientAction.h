#pragma once

class ClientAction {
private:
	int src;
	int dest;
	int unit_counts;
public:
	ClientAction(int src, int dest, int unit_counts) {
		this->src = src;
		this->dest = dest;
		this->unit_counts = unit_counts;
	}
	int getSrc() {
		return src;
	}
	int getDest() {
		return dest;
	}
	int getUnitCounts() {
		return unit_counts;
	}
};