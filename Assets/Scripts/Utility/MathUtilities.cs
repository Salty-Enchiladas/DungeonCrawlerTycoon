//DOCUMENTATION:    https://github.com/OuterRimStudios/Utilities/wiki/Math-Utilities

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OuterRimStudios.Utilities
{
    public static class MathUtilities
    {
        #region MapValue

        /// <summary>Maps a value that exists between a range to be within a 0 to 1 range.</summary>
        /// <param name="currentRangeMin">The minimum value of the range that the value currently exists.</param>
        /// <param name="currentRangeMax">The maximum value of the range that the value currently exists.</param>
        /// <param name="value">The value to be mapped within the new range.</param>
        /// <returns>Returns input value converted to a value within a 0 to 1 range.</returns>
        public static float MapValue01(float currentRangeMin, float currentRangeMax, float value)
        {
            return currentRangeMin + (currentRangeMax - currentRangeMin) * ((value - 0) / (1 - 0));
        }

        /// <summary>Maps a value that exists between a range to be within a new range.</summary>
        /// <param name="newRangeMin">The minimum value of the range within which the value will be mapped.</param>
        /// <param name="newRangeMax">The maximum value of the range within which the value will be mapped.</param>
        /// <param name="currentRangeMin">The minimum value of the range that the value currently exists.</param>
        /// <param name="currentRangeMax">The maximum value of the range that the value currently exists.</param>
        /// <param name="value">The value to be mapped within the new range.</param>
        /// <returns>Returns input value converted to a value within new range.</returns>
        public static float MapValue(float newRangeMin, float newRangeMax, float currentRangeMin, float currentRangeMax, float value)
        {
            return currentRangeMin + (currentRangeMax - currentRangeMin) * ((value - newRangeMin) / (newRangeMax - newRangeMin));
        }

        #endregion MapValue

        #region Clamp

        /// <summary>Clamps all axes of the entered Vector3 between the negative and positive of the entered clamp value.</summary>
        /// <param name="vector">The Vector3 to be clamped.</param>
        /// <param name="clampValue">The value at which the Vector3 will be clamped.</param>
        /// <returns>Returns the Vector3 after being clamped within the clamp range.</returns>
        public static Vector3 Clamp(Vector3 vector, float clampValue)
        {
            return new Vector3(Mathf.Clamp(vector.x, -clampValue, clampValue), Mathf.Clamp(vector.y, -clampValue, clampValue), Mathf.Clamp(vector.z, -clampValue, clampValue));
        }

        /// <summary>Clamps all axes of the entered Vector3 between the minClampValue and the maxClampValue.</summary>
        /// <param name="vector">The Vector3 to be clamped.</param>
        /// <param name="minClampValue">The minimum value at which the Vector3 will be clamped.</param>
        /// <param name="maxClampValue">The maximum value at which the Vector3 will be clamped.</param>
        /// <returns>Returns the Vector3 after being clamped between the minClampValue and maxClampValue.</returns>
        public static Vector3 Clamp(Vector3 vector, float minClampValue, float maxClampValue)
        {
            return new Vector3(Mathf.Clamp(vector.x, minClampValue, maxClampValue), Mathf.Clamp(vector.y, minClampValue, maxClampValue), Mathf.Clamp(vector.z, minClampValue, maxClampValue));
        }

        #endregion Clamp

        #region CheckDistance

        /// <summary>Calculates the distance between two Vector3 points.</summary>
        /// <param name="point1">The first point of the distance calculation.</param>
        /// <param name="point2"> The second point of the distance calculation.</param>
        /// <returns>Return the distance between the two points.</returns>
        public static float CheckDistance(Vector3 point1, Vector3 point2)
        {
            Vector3 heading;
            float distance;
            Vector3 direction;
            float distanceSquared;

            heading.x = point1.x - point2.x;
            heading.y = point1.y - point2.y;
            heading.z = point1.z - point2.z;

            distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;
            distance = Mathf.Sqrt(distanceSquared);

            direction.x = heading.x / distance;
            direction.y = heading.y / distance;
            direction.z = heading.z / distance;
            return distance;
        }

        /// <summary>Calculates the distance between two Vector2 points.</summary>
        /// <param name="point1">The first point of the distance calculation.</param>
        /// <param name="point2">The second point of the distance calculation.</param>
        /// <returns>Return the distance between the two points.</returns>
        public static float CheckDistance(Vector2 point1, Vector2 point2)
        {
            Vector2 heading;
            float distance;
            Vector2 direction;
            float distanceSquared;

            heading.x = point1.x - point2.x;
            heading.y = point1.y - point2.y;

            distanceSquared = heading.x * heading.x + heading.y * heading.y;
            distance = Mathf.Sqrt(distanceSquared);

            direction.x = heading.x / distance;
            direction.y = heading.y / distance;
            return distance;
        }

        #endregion CheckDistance

        #region GetDivisors

        /// <summary> Get all of the divisors of a dividend</summary>
        /// <param name="dividend">the number to be divided by another number</param>
        /// <returns>list of ints that the dividend cleanly divides with</returns>
        public static List<int> GetDivisors(int dividend)
        {
            List<int> divisors = new List<int>();
            for (int i = 1; i <= dividend; i++)
            {
                if (dividend % i == 0)
                    divisors.Add(i);
            }
            return divisors;
        }

        #endregion GetDivisors

        #region GetDistribution
        /// <summary>Distributes an initialValue into a random amount of distributives with unequal values.</summary>
        /// <param name="initialValue">The value to be distributed.</param>
        /// <param name="maxDistributives">The max number of values to be returned.</param>
        /// <param name="minDistributiveValue">The min value a distributive can be.</param>
        /// <param name="maxDistributiveValue">The max value a distributive can be.</param>
        /// <param name="chanceIncrease">The value to add to a Distributives rollChance on a failed roll.</param>
        /// <returns>Returns a list of integers that will be randomly distributed. The sum of the values will equal the paramref name="initialValue"</returns>
        public static List<int> GetRandomDistribution(int initialValue, int maxDistributives, int minDistributiveValue, int maxDistributiveValue, int chanceIncrease)
        {
            int minDistributives = 1;
            //Divide initialValue by maxDistributiveValue when initialValue is greater than maxDistributiveValue to ensure that there are enough distributives to allocate values to.
            if (initialValue > maxDistributiveValue)
                minDistributives = Mathf.CeilToInt(initialValue / (float)maxDistributiveValue);

            //Get a random number of distributives to allocate values to.
            int distributions = Random.Range(minDistributives, maxDistributives + 1);

            //Get the max number each distributive could be if the initialValue was distributed evenly
            int distributionMax = initialValue / distributions;

            List<Distributive> distributives = new List<Distributive>();
            List<int> results = new List<int>(distributions);

            //Loop a number of times equal to the number of distributives to return
            for (int i = 0; i < distributions; i++)
            {
                //Get a random value to allocate that is between the minimum value a distributive can be and the distributionMax
                int distributiveStartValue = Random.Range(minDistributiveValue, distributionMax);
                results.Add(distributiveStartValue);
                //Remove the random value that will be allocated from the initialValue
                initialValue -= distributiveStartValue;
                //Allocate the random value and initialize that Distributives rollChance
                distributives.Add(new Distributive() { value = distributiveStartValue, rollChance = (100 - (int)((float)(distributiveStartValue / maxDistributiveValue) * 100)) });
            }

            //Randomly distributes the remaining value of initialValue
            CoroutineUtilities.Instance.StartCoroutine(RandomDistribution(initialValue, distributives, maxDistributiveValue, chanceIncrease));

            //Applies all of the values to the results List after initialValue has been randomly distributed
            for (int i = 0; i < distributions; i++)
                results[i] = distributives[i].value;

            return results;
        }

        /// <summary>Randomly distrtibutes paramref name="initialValue" between the distributives</summary>
        /// <param name="initialValue">The value to be distributed.</param>
        /// <param name="distributives">list of Distributives to allocate to.</param>
        /// <param name="maxDistributiveValue">The max value a distributive can be.</param>
        /// <param name="chanceIncrease">The value to add to a Distributives rollChance on a failed roll.</param>
        /// <returns></returns>
        public static IEnumerator RandomDistribution(int initialValue, List<Distributive> distributives, int maxDistributiveValue, int chanceIncrease)
        {
            //Loop until all of initialValue has been allocated
            for (; ; )
            {
                //if initialValue is 0, then it has been totally allocated. break out of loop
                if (initialValue == 0) break;

                //loop through each distributive in distributives list
                for (int distributive = 0; distributive < distributives.Count; distributive++)
                {
                    //if initialValue is 0, then it has been totally allocated. break out of loop
                    if (initialValue == 0) break;
                    //if the rollChance of a distributive is 0, then that distributive has reached the max value. ignore this distributive and continue to the next iteration of the loop
                    if (distributives[distributive].rollChance == 0) continue;
                    //Get a random value between 0-100 to compare to rollChance
                    int roll = Random.Range(0, 101);
                    //if the random roll value is less than or equal to the distributives rollChance, then allocate a point, recalculate rollChance, and decrement initialValue
                    if (roll <= distributives[distributive].rollChance)
                    {
                        distributives[distributive].value += 1;
                        distributives[distributive].rollChance = (100 - (int)((float)(distributives[distributive].value / maxDistributiveValue) * 100));
                        initialValue--;
                    }
                    else    //If the roll is greater than the distributives rollChance, then increase the distributives rollChance
                        distributives[distributive].rollChance += chanceIncrease;
                }
            }
            yield break;
        }
        #endregion
    }

    /// <summary>Stores the current value and the current rollChance of the Distributive.</summary>
    public class Distributive
    {
        public int value;
        public int rollChance;
    }
}