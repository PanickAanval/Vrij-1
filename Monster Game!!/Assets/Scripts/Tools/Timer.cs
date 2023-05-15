using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Timer
{
    public float time = 0f;
    public float timer = 0f;

    public float percent
    {
        get
        {
            if (time == 0) return 1f;
            return timer / time;
        }
    }

    public Timer(float time)
    {
        this.time = time;
    }

    public bool HasReached(float deltaTime)
    {
        timer += deltaTime;
        if (timer < time) return false;
        Reset();
        return true;
    }

    public void Reset()
    {
        time = 0f;
        timer = 0f;
    }

    public void Reset(float newTime)
    {
        time = newTime;
        timer = 0f;
    }
}
