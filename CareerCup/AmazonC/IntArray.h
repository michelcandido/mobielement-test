#include <iostream>
#include <math.h>
using namespace std;

class IntArray
{
public:
	void swapSort(int src[], int size);
	void maxSubArray(int src[], int size);
	void matrixTransposition(int src[], int N);
	int findMinDistance(int src[], int size, int x, int y);
	void maxOfSubArray(int src[], int size, int k);

};

void IntArray::maxOfSubArray(int src[], int size, int k) {
	int max = 0;
	int i = 0;
	for (i = 0; i < k; i++)
	{
		if (src[i] > max)
			max = src[i];
	}
	cout << max << " ";
	for (; i < size; i++)
	{
		if (src[i] > max)
			max = src[i];
		cout << max << " ";
	}
}

int IntArray::findMinDistance(int src[], int size, int x, int y)
{
	int min = size;
	int prev;
	int i = 0;
	for (i = 0; i < size; i++)
	{
		if (src[i] == x || src[i] == y)
		{
			prev = i;
			break;
		}
	}
	for (; i < size; i++)
	{
		if (src[i] == x || src[i] == y) {
			if (src[i] != src[prev] && (i - prev) < min) {
				min = i - prev;
			}
			prev = i;
		}
	}

	return min;
	/*
	int min = size;
	for (int i = 0; i < size; i++)
	{
		if (src[i] == x)
		{
			int pos = 1;
			int left = i - pos, right = i + pos;
			while (left >= 0 || right < size)
			{
				if (left < 0) left = 0;
				if (right >= size) right = size - 1;
				if (src[left] == y || src[right] == y)
					if (pos < min)
						min = pos;
				pos++;
				left--;
				right++;
			}
		}
	}
	return min;
	*/
}

void IntArray::swapSort(int src[], int size)
{
	int min = src[0], mid = src[0], max = src[0];
	for (int i = 0; i < size; i++)
	{
		if (src[i] == min || src[i] == mid || src[i] == max)
			continue;
		if (src[i] < min)
		{
			max = mid;
			mid = min;
			min = src[i];
			continue;
		}
		if (src[i] > min && src[i] < mid)
		{
			max = mid;
			mid = src[i];
			continue;
		}
		if (src[i] > mid)
		{
			max = src[i];
			continue;
		}
	}
	cout << min << " " << mid << " " << max << endl;
	int pos = -1;
	for (int i = 0; i < size; i++)
	{
		if (src[i] == min)
		{
			pos++;
			swap(src[i], src[pos]);
		}
	}
	for (int i = 0; i < size; i++)
	{
		cout << src[i] << " ";
	}
	cout << endl;
	pos = size;
	for (int i = size - 1; i >= 0; i--)
	{
		if (src[i] == max)
		{
			pos--;
			swap(src[i], src[pos]);
		}
	}
	for (int i = 0; i < size; i++)
	{
		cout << src[i] << " ";
	}
	cout << endl;

}

void IntArray::maxSubArray(int src[], int size)
{
	int maxSoFar = 0, maxEndingHere = 0, start = 0, end = 0;
	for (int i = 0; i < size; i++)
	{
		maxEndingHere += src[i];
		if (maxEndingHere < 0 ) {
			maxEndingHere = 0;
			end = i+1;
			start = i+1;
		}
		if (maxEndingHere > maxSoFar) {
			maxSoFar = maxEndingHere;
			end = i;
		}
	}
	cout << src[start] << " " << src[end] << " " << maxSoFar << endl;
}

bool isPalindrome(const char *src, int start, int end) 
{
	if (end < start) return true;
	if (*(src+start) == *(src+end))
		return isPalindrome(src, start + 1, end - 1);
	else
		return false;
}

void IntArray::matrixTransposition(int src[], int N)
{
	for (int i = 0; i < N; i++) {
		for (int j = 0; j < N; j++) {
			cout << src[i*N+j] << " ";
		}
		cout << endl;
	}
	cout << endl;
	
	for (int n = 0; n <= N -2;n++) {
		for (int m = n + 1; m <= N - 1; m++) {
			swap(src[n*N+m], src[m*N+n]);
		}
	}

	for (int i = 0; i < N; i++) {
		for (int j = 0; j < N; j++) {
			cout << src[i*N+j] << " ";
		}
		cout << endl;
	}
}