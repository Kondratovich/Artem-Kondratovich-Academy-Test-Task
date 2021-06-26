namespace Artem_Kondratovich_Academy_Test_Task {
    class Program {
        static void Main(string[] args) {
			int[,] map = new int[,] { { 1,9,9,9,9 }, { 1,9,1,1,1 }, { 1, 1, 1, 2, 2 }, { 9, 9, 9, 9, 1 } };
			Rover.CalculateRoverPath(map);
		}

    }
}
