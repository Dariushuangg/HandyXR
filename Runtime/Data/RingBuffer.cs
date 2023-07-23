using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simplified ring buffer implementation for storing the streamed XRHand data and hand feature data.
/// </summary>
public class RingBuffer<Data>
{
    public Data[] _datas { private set; get; }
    public int _capacity { private set; get; }
    public int _index { private set; get; } // index of the last written data

    public RingBuffer()
    {
        int fps = 120;
        int maxGestureTime = 5;
        _capacity = fps * maxGestureTime;
        _datas = new Data[_capacity];
        _index = 0;
    }
    public RingBuffer(int capacity)
    {
        _capacity = capacity;
        _datas = new Data[_capacity];
        _index = 0;
    }
    public void Append(Data data) { 
        _index = (_index + 1) % _capacity;
        _datas[_index] = data;
    }

    /// <summary>
    /// Clear the entire buffer; Called when turning off the gesture recognition functionality completely.
    /// </summary>
    public void Clear() { _datas = new Data[_capacity];  _index = 0; }

    /// <summary>
    /// Return a chunk of the past <paramref name="frameCount"/> frames of data starting at index <paramref name="startIndex"/>
    /// for continuous gesture validation etc.
    /// </summary>
    /// <param name="startIndex">The index when the start gesture is detected.</param>
    /// <param name="frameCount">The number of frame after the start gesture is performed.</param>
    /// <returns></returns>
    public Data[] GetInterval(int startIndex, int frameCount)
    {
        Data[] interval = new Data[frameCount];
        for (int i = 0; i < frameCount; i++) 
        {
            int idx = startIndex - i;
            if (idx < 0) idx += _capacity;
            interval[i] = _datas[idx];
        }
        return interval;
    }

    /// <summary>
    /// Return the last <paramref name="frameCount"/> frames of data for continuous gesture validation etc.
    /// </summary>
    /// <param name="frameCount"></param>
    /// <returns></returns>
    public Data[] GetInterval(int frameCount) 
    {
        return GetInterval(_index, frameCount);
    }
}
