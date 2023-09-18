
public class Player
{
    private int[] position; 
    public int[] Position => position;
    private int[] direction;
    public int[] Direction => direction;

    public Player(int[] position, int[] direction) {
        this.position = position;
        this.direction = direction;
    }
}