/*public class PlayerState
{
    public bool isCollidingRight { get; set; }
    public bool isCollidingLeft { get; set; }
    public bool isCollidingBelow { get; set; }
    public bool isCollidingAbove { get; set; }
    public bool isGrounded { get { return isCollidingBelow; } }
    public bool hasCollisions { get { return isCollidingRight || isCollidingLeft || isCollidingAbove || isCollidingBelow; } }

    public void Reset()
    {
        isCollidingLeft = false;
        isCollidingRight = false;
        isCollidingAbove = false;
        isCollidingBelow = false;
    }

    public override string ToString()
    {
        return string.Format("(controller: r:{0} l:{1} a:{2} b:{3})",
            isCollidingRight,
            isCollidingLeft,
            isCollidingAbove,
            isCollidingBelow);
    }
}
*/