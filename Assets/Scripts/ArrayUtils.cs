
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mimic {
	
	public static class ArrayUtils {

		private static Random rnd = new Random();  

		public static List<T> Clone<T>(this List<T> listToClone) where T: ICloneable {
			return listToClone.Select(item => (T)item.Clone()).ToList();
		}

		public static bool IsEmpty(this IList list) {
			return list.Count == 0;
		}

		public static T GetRandomElement<T>(this T[] array) {
			return array[rnd.Next(array.Length)];
		}

		public static T GetRandomElement<T>(this List<T> list) {
			return list.IsEmpty() ? default(T) : list[rnd.Next(list.Count)];
		}

		public static T FindRandom<T>(this List<T> list, Predicate<T> match) {
			if(list.IsEmpty()){
				return default(T);
			} else {
				List<T> filteredList = list.FindAll(match);
				return filteredList.GetRandomElement();
			}
		}

		public static T Remove<T>(this IList<T> list, int index) {
			T removedElement = list[index];
			list.RemoveAt(index);
			return removedElement;
		}

		public static T RemoveElementAtRandom<T>(this IList<T> list) {
			return list.Remove(rnd.Next(list.Count));
		}

		public static bool MoveElementUp<T>(this List<T> list, T element) {
			int index = list.IndexOf(element);
			if(index > 0) {
				list[index] = list[index - 1];
				list[index - 1] = element;
				return true;
			} else {
				return false;
			}
		}

		public static bool MoveElementDown<T>(this List<T> list, T element) {
			int index = list.IndexOf(element);
			if(index < list.Count - 1) {
				list[index] = list[index + 1];
				list[index + 1] = element;
				return true;
			} else {
				return false;
			}
		}

		public static void Shuffle<T>(this IList<T> list) {  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = rnd.Next(n + 1);  
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}  
		}

		public static T FindMinimum<T>(this IList<T> list) where T:IComparable<T> {
			int listCount = list.Count;
			T minimum = list[0], currentElement;
			for (int i = 1; i < listCount; i++) {
				currentElement = list[i];
				if (currentElement.CompareTo(minimum) < 0) {
					minimum = currentElement;
				}
			}
			return minimum;
		}

		public static T FindMaximum<T>(this IList<T> list) where T : IComparable<T> {
			int listCount = list.Count;
			T maximum = list[0], currentElement;
			for (int i = 1; i < listCount; i++) {
				currentElement = list[i];
				if (currentElement.CompareTo(maximum) > 0) {
					maximum = currentElement;
				}
			}
			return maximum;
		}

	}

}