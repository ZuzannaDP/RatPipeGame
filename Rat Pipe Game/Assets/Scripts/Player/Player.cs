
public class Player
{
    private Position position; 
    public Position Position => position;
    private Direction facing;
    public Direction Facing => facing;

    public Player(Position position, Direction direction) {
        this.position = position;
        facing = direction;
    }
}