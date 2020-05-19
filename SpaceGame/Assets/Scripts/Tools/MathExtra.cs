using System;
public static class MathExtra
{
    //https://stackoverflow.com/a/8328226/5487005
    public static float GetMedian(float[] sourceNumbers) {
        if (sourceNumbers == null || sourceNumbers.Length == 0)
            throw new System.Exception("Median of empty array not defined.");

        //make sure the list is sorted, but use a new array
        float[] sortedPNumbers = (float[])sourceNumbers.Clone();
        Array.Sort(sortedPNumbers);

        //get the median
        int size = sortedPNumbers.Length;
        int mid = size / 2;
        float median = (size % 2 != 0) ? (float)sortedPNumbers[mid] : ((float)sortedPNumbers[mid] + (float)sortedPNumbers[mid - 1]) / 2;
        return median;
    }
}
