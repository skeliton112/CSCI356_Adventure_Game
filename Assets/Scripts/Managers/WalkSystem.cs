using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {
	float[][] edges;

	public int Size {
		get {
			return edges.Length + 1;
		}
	}

	public float this[int i, int j]{
		get {
			if (i == j) {
				return -1;
			} else if (i < j) {
				return edges [j - 1] [i];
			} else {
				return edges [i - 1] [j];
			}
		}
		set {
			if (i < j) {
				edges [j - 1] [i] = value;
			} else if (i > j) {
				edges [i - 1] [j] = value;
			}
		}
	}

	public Graph (int size){
		edges = new float[size - 1][];
		for (int i = 0; i < size - 1; i++) {
			edges [i] = new float[i + 1];
		}
	}
}

public class Polygon {
	public List<Vector2> verts = new List<Vector2>();

	public Vector2 this[int i]{
		get {
			return verts [i];
		}
	}

	public Polygon (List<Vector2> v){
		verts = v;
	}

	public int Count {
		get {
			return verts.Count;
		}
	}

	double cross_product (Vector2 a, Vector2 b){
		return a.x * b.y - a.y * b.x;
	}

	public bool crosses (Vector2 a_1, Vector2 b_1, Vector2 a_2, Vector2 b_2, bool count_colinear){
		Vector2 d_1 = b_1 - a_1;
		Vector2 d_2 = b_2 - a_2;

		double det_1 = cross_product ((a_2 - a_1), d_2);
		double det_2 = cross_product ((a_2 - a_1), d_1);
		double det_3 = cross_product (d_1, d_2);

		if (det_3 == 0) {
			if (det_2 == 0) { //Colinear -> maybe intersecting.  Check
				if (a_1 == b_1) {
					return Vector2.Dot (a_1 - a_2, d_2) <= Vector2.Dot (d_2, d_2) && count_colinear;
				}
				if (a_2 == b_2) {
					return Vector2.Dot (a_2 - a_1, d_1) <= Vector2.Dot (d_1, d_1) && count_colinear;
				}

				double t_0 = Vector2.Dot(a_2 - a_1, d_1) / Vector2.Dot(d_1, d_1);
				double t_1 = Vector2.Dot (b_2 - a_1, d_1) / Vector2.Dot (d_1, d_1);

				if (Vector2.Dot (d_1, d_2) > 0) { //Positively oriented
					return t_0 < 1 && t_1 > 0 && count_colinear;
				} else {
					return t_1 < 1 && t_0 > 0 && count_colinear;
				}
			} else { //Parallel -> non-intersecting
				return false;
			}
		} else {
			//Check if collision point is inside both intervals
			return (det_2 / det_3 > 0 && det_2 / det_3 < 1) && (det_1 / det_3 > 0 && det_1 / det_3 < 1);
		}
	}

	public bool contains (Vector2 a, bool on_line){
		bool parity = false;
		for (int i = 0; i < verts.Count; i++) {
			
			int j = (i + 1) % verts.Count; 

			float frac = Vector2.Dot(a - verts [i], verts [j] - verts [i]) / Vector2.SqrMagnitude (verts [j] - verts [i]);
			if (frac >= 0 && frac <= 1) { //test if the point is on the line
				if (Mathf.Abs (Vector2.Dot (a - verts [i], new Vector2 (verts [j].y - verts [i].y, verts [i].x - verts [j].x))) <= float.Epsilon) {
					return on_line;
				}
			}

			if (((verts [i].y <= a.y) ^ (verts [j].y < a.y)) //Test if line crosses
				&& //If it does, test if it crosses on the right side
				((verts [i].y - a.y) / (verts [i].y - verts [j].y) * (verts [j].x - verts [i].x) + verts [i].x <= a.x)){

				parity = !parity; //Flip parity
			}
		}
		return parity;
	}

	public bool contains (Vector2 a, Vector2 b){
		for (int i = 0; i < verts.Count; i++) {
			if (crosses (a, b, verts [i], verts [(i + 1) % verts.Count], false))
				return false;
		}
		return contains ((a + b) / 2, true);
	}

	public bool intersects (Vector2 a, Vector2 b){
		for (int i = 0; i < verts.Count; i++) {
			if (crosses (a, b, verts [i], verts [(i + 1) % verts.Count], false))
				return true;
		}
		return contains ((a + b) / 2, false);
	}
}

public class Region {
	Polygon boundary;
	List<Polygon> holes;
	public int count { get; set;}
	public Graph graph;
	Vector3 normal, right;
	float constant;

	public Region (Polygon p, List<Polygon> h, Vector3 norm, Vector3 rght, float offset) {
		boundary = p;
		holes = h;
		Vector3.OrthoNormalize(ref norm, ref rght);
		normal = norm;
		right = rght;
		constant = offset;

		count = boundary.Count;
		for (int i = 0; i < holes.Count; i++) {
			count += holes [i].Count;
		}

		graph = new Graph (count);
	}

	public Vector2 this[int i]{
		get {
			if (i < boundary.Count)
				return boundary [i];
			else {
				i -= boundary.Count;
				for (int j = 0; j < holes.Count; j++){
					if (i < holes [j].Count)
						return holes [j] [i];
					i -= holes [j].Count;
				}
			}
			return Vector2.zero;
		}
	}

	public Vector2 get_neighbour (int i){
		if (i < 0) {
			return Vector2.zero;
		} else if (i == boundary.Count - 1) {
			return boundary [0];
		} else if (i < boundary.Count) {
			return boundary [i + 1];
		} else {
			i -= boundary.Count;
			for (int j = 0; j < holes.Count; j++){
				if (i == holes [j].Count - 1) {
					return holes [j] [0];
				} else if (i < holes [j].Count) {
					return holes [j] [i + 1];
				}
				i -= holes [j].Count;
			}
		}
		return Vector2.zero;
	}

	public bool are_neighbours (int i, int j){ //Assumes i < j
		if (i < boundary.Count && j < boundary.Count)
			return i+1 == j || i == 0 && j == boundary.Count - 1;
		else {
			i -= boundary.Count; j -= boundary.Count;
			if (i < 0)
				return false;
			
			for (int k = 0; k < holes.Count; k++){
				if (i < holes [k].Count && j < holes[k].Count)
					return i+1 == j || i == 0 && j == holes[k].Count - 1;
				
				i -= holes [k].Count;
				j -= holes [k].Count;
				if (i < 0)
					return false;
			}
		}
		return false;
	}

	public void build_graph () {
		for (int i = 0; i < count; i++) {
			for (int j = 0; j < i; j++) {
				if (are_neighbours(j, i) || contains (this [i], this [j]))
					graph [i, j] = Vector2.Distance (this [i], this [j]);
				else
					graph [i, j] = -1;
			}
		}
		for (int i = 0; i < graph.Size; i++) {
			string val = "";
			for (int j = 0; j < graph.Size; j++) {
				val += graph [i, j] + "\t";
			}
		}
	}

	public float[,] connect_temporary_verts (Vector2 a, Vector2 b){
		float[,] temp_mat = new float [2, count];
		for (int i = 0; i < count; i++) {
			if (contains (this [i], a))
				temp_mat [0, i] = Vector2.Distance (a, this [i]);
			else
				temp_mat [0, i] = -1;
			if (contains (this [i], b))
				temp_mat [1, i] = Vector2.Distance (b, this [i]);
			else
				temp_mat [1, i] = -1;
		}
		return temp_mat;
	}

	public bool contains (Vector2 point){
		if (boundary.contains (point, true)) {
			foreach (Polygon hole in holes) {
				if (hole.contains (point, false))
					return false;
			}
			return true;
		}
		return false;
	}

	public bool contains (Vector2 a, Vector2 b){
		if (boundary.contains (a, b)) {
			foreach (Polygon hole in holes) {
				if (hole.intersects (a, b))
					return false;
			}
			return true;
		} else {
			return false;
		}
	}

	float square (float a){
		return a * a;
	}

	public float sqrdistance (Vector2 a, Vector2 b, Vector2 p) {
		float frac = Vector2.Dot(p - a, b - a) / Vector2.SqrMagnitude (b - a);
		if (0 < frac && frac < 1){
			float numerator = square ((b.y - a.y)*p.x - (b.x - a.x)*p.y + b.x*a.y - a.x*b.y);
			float denominator = square(b.y - a.y) + square(b.x - a.x);

			return numerator / denominator;
		}else{
			return Mathf.Min (Vector2.SqrMagnitude (a - p), Vector2.SqrMagnitude (b - p));
		}
	}

	public Vector2 project (Vector2 a, Vector2 b, Vector2 p) {
		float frac = Vector2.Dot(p - a, b - a) / Vector2.SqrMagnitude (b - a);

		if (0 < frac && frac < 1){
			return frac * (b - a) + a;
		}else{
			return Vector2.SqrMagnitude (a - p) < Vector2.SqrMagnitude (b - p) ? a : b;
		}
	}

	public Vector2 project (Vector3 vec){
		Vector3 displacement = vec - normal * constant;
		float error = Vector3.Dot (displacement, normal);
		Vector2 planar_vec = new Vector2 (Vector3.Dot (vec, right), Vector3.Dot (vec, Vector3.Cross (right, normal)));

		if (contains (planar_vec)) {
			return planar_vec;
		}

		int minimum = 0;
		float min_sqr_distance = -1;
		for (int i = 0; i < count; i++) {
			float new_dist = sqrdistance (this [i], get_neighbour (i), planar_vec);
			if (min_sqr_distance == -1 || new_dist < min_sqr_distance) {
				min_sqr_distance = new_dist;
				minimum = i;
			}
		}

		error = Mathf.Sqrt (square (error) + min_sqr_distance);

		return project (this [minimum], get_neighbour (minimum), planar_vec);
	}

	public Vector3 embed (Vector2 vec){
		return normal * constant + right * vec.x + Vector3.Cross (right, normal) * vec.y;
	}
}

public class WalkSystem {

	public Region region;

	public float h (int i, Vector2 goal){
		if (i >= 0)
			return Vector2.Distance (region [i], goal);
		return 0;
	}

	public List<Vector3> get_path (Vector3 start, Vector3 finish){
		Vector2 start_2d = project (start);
		Vector2 finish_2d = project (finish);

		if (region.contains (start_2d, finish_2d)) {
			return new List<Vector3> () { region.embed (finish_2d) };
		} else {
			return A_Star (finish_2d, region.connect_temporary_verts (start_2d, finish_2d));
		}
	}

	public Vector2 project (Vector3 vec){
		return region.project (vec);
	}

	public List<Vector3> A_Star (Vector2 goal, float[,] temp_edges = null){
		List<int> closed = new List<int> ();
		List<int> open = new List<int> () { 1 };

		int[] came_from = new int[region.count + 2];
		float[] g = new float[region.count + 2];

		for (int i = 0; i < region.count + 2; i++) {
			came_from [i] = -10;
			g [i] = -1;
		}

		g [1] = 0;

		while (open.Count > 0) {
			int current = open[0];

			for (int i = 1; i < open.Count; i++) {
				if (g [open[i]] + h (open[i] - 2, goal) < g [current] + h (current - 2, goal)) {
					current = open[i];
				}
			}

			if (current == 0) {
				List<Vector3> path = new List<Vector3> ();
				path.Add (region.embed (goal));

				current = came_from [current];
				while (current != 1) {
					path.Add (region.embed (region[current - 2]));
					current = came_from [current];
				}

				path.Reverse ();
				return path;
			}

			open.Remove (current);
			closed.Add (current);

			for (int i = 0; i < region.count; i++) {
				if ((current > 1 ? region.graph [i, current - 2] : temp_edges[1 - current, i]) >= 0) {
					if (closed.Contains (i + 2))
						continue;
					if (!open.Contains (i + 2))
						open.Add (i + 2);

					float tentative_g = g [current] + (current > 1 ? region.graph [i, current - 2] : temp_edges[1 - current, i]);
					if (tentative_g > g [i + 2] && g [i + 2] >= 0)
						continue;

					g [i + 2] = tentative_g;
					came_from [i + 2] = current;
				}
			}
			if (current > 1 && temp_edges [1, current - 2] >= 0) {
				if (!open.Contains (0))
					open.Add (0);

				float tentative_g = g [current] + temp_edges[1, current - 2];
				if (tentative_g < g [0] || g [0] < 0) {
					g [0] = tentative_g;
					came_from [0] = current;
				}
			}
		}
		return null;
	}

	private WalkSystem () {
		region = new Region (
			new Polygon(new List<Vector2>(){
				new Vector2(-2, -2),
				new Vector2(-2,  2),
				new Vector2( 4,  2),
				new Vector2( 4,  0),
				new Vector2( 3,  0),
				new Vector2( 3,  1),
				new Vector2( 2,  1),
				new Vector2( 2, -2),
			}),
			new List<Polygon>() {
				new Polygon(new List<Vector2>(){
					new Vector2(-1, -1),
					new Vector2(-1,  1),
					new Vector2( 1,  1),
					new Vector2( 1, -1),
				})
			},
			Vector3.up,
			Vector3.right,
			0
		);
		region.build_graph ();
	}

	//Singleton pattern
	private static WalkSystem instance = null;
	public static WalkSystem Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new WalkSystem();
			}
			return instance;
		}
	}
}
