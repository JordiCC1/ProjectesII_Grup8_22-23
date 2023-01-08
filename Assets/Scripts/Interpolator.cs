using System;

public class Interpolator
{
    public enum State { MIN, MAX, SHRINKING, GROWING }

    private State state;

    private float currentTime = 0.0f;
    private readonly float maxTime;

    public float Value { get; private set; } = .0f;
    public float Inverse { get { return 1 - Value; } }

    private readonly float epsilon = .05f;
    public bool IsMaxPrecise { get { return state == State.MAX; } }
    public bool IsMinPrecise { get { return state == State.MIN; } }

    public bool IsMax { get { return Value > 1f - epsilon; } }
    public bool IsMin { get { return Value < epsilon; } }

    public Interpolator(float lerpTime)
    {
        maxTime = lerpTime;
        state = State.MIN;
    }

    public void ToMax()
    {
        state = state != State.MAX ? State.GROWING : State.MAX;
    }

    public void ToMin()
    {
        state = state != State.MIN ? State.SHRINKING : State.MIN;
    }

    public void ForceMax()
    {
        currentTime = maxTime;
        Value = 1f;
        state = State.MAX;
    }

    public void ForceMin()
    {
        currentTime = 0f;
        Value = 0f;
        state = State.MIN;
    }

    public void Update(float dt)
    {
        if (state == State.MIN || state == State.MAX)
            return;
        float modifiedDT = state == State.GROWING ? dt : -dt;

        currentTime = currentTime + modifiedDT;

        if (currentTime >= maxTime)
            ForceMax();
        else if (currentTime <= .0f)
            ForceMin();
        else
            Value = currentTime / maxTime;
    }
}
