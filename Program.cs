using System;

// Point class holds the details for each point entry
public class Point{
    public int num;
    public double x;
    public double y;
    public string dir;

    public Point(int num, double x, double y, string dir) {
        this.num = num;
        this.x = x;
        this.y = y;
        this.dir = dir;
        }
}

// Primary logic for the vision cones
public class Program{
    public Point[] points = new Point[20];
    public double[] xPoints = new double[] {28,27,16,40,8,6,28,39,12,36,22,33,41,41,14,6,46,17,28,2};
    public double[] yPoints = new double[] {42,46,22,50,6,19,5,36,34,20,47,19,18,34,29,49,50,40,26,12};
    public string[] dirPoints = new string[] {"North", "East", "South", "West", "North", "East", "South", "West", "North", "East", "South", "West", "North", "East", "South", "West", "North", "East", "South", "West"};

    // Runs program
    public static void Main(string[] args){
        Program program = new Program();
        List<Point> results = new List<Point>();

        program.InitPoints();
        results = program.VisiblePoint(1,45,20);
        program.PrintPoints(results);
        return;
    }

    // Initialise points
    public void InitPoints(){
        for(int i = 0; i < 20; i++){
            points[i] = new Point(i+1, xPoints[i], yPoints[i], dirPoints[i]);
        }

    }

    // Traverses points to see if they fall within the vision cone
    public List<Point> VisiblePoint(int pointNo, double fov, double dist){
        List<Point> visiblePoints = new List<Point>();
        Point viewPoint = points[pointNo-1];
        Point currentPoint;
        double x1 = 0;
        double y1 = 0;
        
        // Converted degrees to radians as Math.Acos uses radians
        fov = fov * (Math.PI / 180);

        // Set up vectors for dot product rule to calculate angle between
        switch(viewPoint.dir){
            case "North":
                y1 = dist;
                break;
            case "East":
                x1 = dist;
                break;
            case "South":
                y1 = -dist;
                break;
            case "West":
                x1 = -dist;
                break;
        }

        // Traverse points
        for(int i = 0; i < 20; i++){
            currentPoint = points[i];
            
            // Check conditions to add to viewability list
            // Ensure not the same point
            if(pointNo-1 == i){
                continue;
            }
            // Out of view distance
            if(Magnitude(currentPoint.x - viewPoint.x, currentPoint.y - viewPoint.y) > dist){
                continue;
            }
            // Within specified angle
            if(calculateAngle(x1, y1, currentPoint.x - viewPoint.x, currentPoint.y - viewPoint.y) > fov){
                continue;
            }

            visiblePoints.Add(currentPoint);
        }

        return visiblePoints;
    }

    // Helper Functions
    // Print list of points found to console
    public void PrintPoints(List<Point> list){
        Point currentPoint;

        for(int i = 0; i < list.Count; i++){
            currentPoint = list[i];
            Console.Write("Point : " + currentPoint.num + ", x : " + currentPoint.x + ", y : " + currentPoint.y + ", Direction : " + currentPoint.dir + "\n");
        }
    }

    // Use dot product to calculate angle between two vectors
    public double calculateAngle(double x1, double y1, double x2, double y2){
        return Math.Acos(DotProduct(x1, y1, x2, y2)/(Magnitude(x1, y1)*Magnitude(x2, y2)));
    }

    // Returns dot product between two vectors
    public double DotProduct(double x1, double y1, double x2, double y2){
        return (x1*x2) + (y1*y2);
    }

    // Returns magnitude of a vector
    public double Magnitude(double x, double y){
        return Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2));
    }
}