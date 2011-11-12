#include <iostream>
#include <math.h>
#include <vector>
#include <stack>
using namespace std;

class IntArray
{
public:
	void swapSort(int src[], int size);
	void maxSubArray(int src[], int size);
	void matrixTransposition(int src[], int N);
	int findMinDistance(int src[], int size, int x, int y);
	void maxOfSubArray(int src[], int size, int k);
	void nextGreaterElement(int src[], int size);
	int binSearchGreater(int src[], int ele, int low, int high);
	int equilibrium(int src[], int n);
	void findDuplicates(int src[], int n);
	vector<vector<int>> findSubSets(int src[], int size, int i);
	void findIntersection(int a1[], int size1, int a2[], int size2);
	void sortStack(stack<int> src);
};

void IntArray::sortStack(stack<int> src) {
	stack<int> dest, temp;
	while (!src.empty()) {
		if (dest.empty()) {
			dest.push(src.top());
			src.pop();
		} else {
			if (src.top() >= dest.top() && (temp.empty() || src.top() <= temp.top())) {
				dest.push(src.top());
				src.pop();
			} else if (src.top() < dest.top()) {
				while (!dest.empty() && src.top() < dest.top()) {
					temp.push(dest.top());
					dest.pop();
				}
				dest.push(src.top());
				src.pop();
			} else if (!temp.empty() && src.top() > temp.top()) {
				while (!temp.empty() && src.top() > temp.top()) {
					dest.push(temp.top());
					temp.pop();
				}
				dest.push(src.top());
				src.pop();
			}
		}
	}
	while (!temp.empty()) {
		dest.push(temp.top());
		temp.pop();
	}
	while (!dest.empty()) {
		cout << dest.top() << endl;
		dest.pop();
	}
}

void IntArray::findIntersection(int a1[], int size1, int a2[], int size2) {
	int p1 = 0, p2 = 0;
	while (p1 < size1 && p2 < size2) {
		if (a1[p1] == a2[p2]) {
			cout << a1[p1] << endl;
			p1++;
			p2++;
		}
		else if (a1[p1] < a2[p2])
			p1++;
		else 
			p2++;
	}
}

vector<vector<int>> IntArray::findSubSets(int src[], int size, int i) {
	vector<vector<int>> subsets;
	vector<int> aset;

	//int size = sizeof(src) / sizeof(int);
	if (i == size - 1) {
		aset.push_back(src[i]);
		subsets.push_back(aset);
		return subsets;
	} 
	vector<vector<int>> sets = findSubSets(src, size, i + 1);
	for (unsigned int j = 0; j < sets.size(); j++) {
		aset = sets[j];
		subsets.push_back(aset);
		aset.push_back(src[i]);
		subsets.push_back(aset);
	}
	vector<int> own;
	own.push_back(src[i]);
	subsets.push_back(own);

	return subsets;
}

void IntArray::findDuplicates(int src[], int n)
{
	long checker = 0;
	for (int i  = 0; i < n; i++) {
		if ((1 << src[i]) & checker) 
			cout << src[i] << " ";
		checker = checker | (1 << src[i]);
	}
	cout << endl;
}

int IntArray::equilibrium(int src[], int n)
{
	int low = src[0];
	int high = 0;
	for (int i = 1; i < n; i++)
		high += src[i];
	for (int i = 1; i < n; i++)
	{
		if (low == high)
			return i;
		if (i + 1 <= n)
		{
			low += src[i+1];
			high -= src[i+1];
		}
	}
	return -1;
}

int IntArray::binSearchGreater(int src[], int ele, int low, int high)
{
	if (low > high) return -1;
	int mid = low + (high - low) / 2;
	if (src[mid] <= ele) {
		return binSearchGreater(src, ele, mid + 1, high);
	} else {
		int t = binSearchGreater(src, ele, low, mid - 1);
		if (t == -1)
			return src[mid];
		else 
			return t;
	}
}

void IntArray::nextGreaterElement(int src[], int size)
{
	for (int i = 0; i < size - 1; i++)
	{
		cout << binSearchGreater(src, src[i], i+1, size-1) << " ";
	}
	cout << -1 << endl;
}

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