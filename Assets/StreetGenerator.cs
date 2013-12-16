﻿using UnityEngine;
using System.Collections;

public class StreetGenerator {
	ArrayList placed;
	ArrayList open;
	int maxRadius;
	
	StreetScript getRandomStreet(StreetScript[] streets, int minPath, int maxPath) {
		ArrayList tmp = new ArrayList();
		foreach(StreetScript s in streets) {
			if(s.getPathsCount() >= minPath && s.getPathsCount() <= maxPath) {
				tmp.Add(s);	
			}
		}
		
		int r = Random.Range(0, tmp.Count);
		StreetScript ret = GameObject.Instantiate((StreetScript)tmp[r]) as StreetScript;
		if(ret == null)
			Debug.Log("generating null street!");
		return ret;
	}
	
	bool isOpen(int x, int y) {
		foreach(StreetScript s in placed) {
			if(s == null)
				continue;
		
			if(s.x == x && s.y == y) {
				if(s.E && !exists (x, y+1))
					return true;
				if(s.W && !exists (x,y-1))
					return true;
				if(s.N && !exists (x+1,y))
					return true;
				if(s.S && !exists (x-1, y))
					return true;
			}
		}
		return false;
	}
	
	bool exists(int x, int y) {
		foreach(StreetScript s in placed) {
			if(s == null)
				continue;
			if(s.x == x && s.y == y)
				return true;
		}
		
		return false;
	}
	
	void placeStreet(StreetScript[] streets) {
		StreetScript s = open[0] as StreetScript;
		StreetScript s2 = null;
		int x2 = s.x;
		int y2 = s.y;
		if(s.W && !exists (s.x,s.y+1))
		{
			y2 = y2+1;
			if(y2 >= maxRadius)
				s2 = getRandomStreet(streets, 1,1);
			else
				s2 = getRandomStreet(streets, 0,4);
			while(!s2.E)
				s2.rotateLeft();
		} else if(s.E && !exists (s.x, s.y-1)) {
			y2 = y2-1;
			if(y2 <= -maxRadius)
				s2 = getRandomStreet(streets, 1,1);
			else
				s2 = getRandomStreet(streets, 0,4);
			while(!s2.W)
				s2.rotateLeft();
		} else if(s.N && !exists (s.x+1, s.y)) {
			x2 = x2+1;
			if(x2 >= maxRadius)
				s2 = getRandomStreet(streets, 1,1);
			else
				s2 = getRandomStreet(streets, 0,4);
			while(!s2.S)
				s2.rotateLeft();
		} else if(s.S && !exists (s.x-1, s.y)) {
			x2 = x2-1;
			if(x2 <= -maxRadius )
				s2 = getRandomStreet(streets, 1,1);
			else
				s2 = getRandomStreet(streets, 0,4);
			
			while(!s2.N)
				s2.rotateLeft();
		} else {
			open.Remove(s);
			return;
		}
		
		s2.x = x2;
			s2.y = y2;
		placed.Add(s2);
		if(isOpen(x2, y2))
			open.Add(s2);
		if(!isOpen (s.x,s.y))
			open.Remove(s);
	}
	
	public ArrayList Generate(StreetScript[] streets, int radius) {
		placed = new ArrayList();
		open = new ArrayList();
		maxRadius = radius;
		
		StreetScript first = getRandomStreet(streets, 2, 4);
		first.x = 0; first.y = 0;
		placed.Add(first);
		open.Add(first);
		int tmp = 0;
		while(open.Count > 0) {
			placeStreet(streets);
			tmp++;
		}
		
		foreach(StreetScript s in placed)
			s.transform.position = new Vector3(25.6f*3*s.x, 0, 25.6f*3*s.y);
		
		return placed;
	}
}
