using System;
using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;

// 작성자 : 양사무엘
// 작성일자 : 2026-09-21
// 공부 주제 : 큐(Queue)
// 목표 : 큐의 개념과 특징과 장단점 이해

class StudyQueue
{
    static void Main()
    {

    }
}

// [큐란?]
// 먼저 들어간 요소가 먼저 나가는 선입선출(FIFO)의 원칙을 따르는 선형 자료구조이다.

// [특징]
// 선입선출 : 먼저 들어간 요소가 먼저 나온다.
// 동적 크기 : 요소가 추가되고 제거됨에 따라 크기가 달라진다.
// 제한된 접근 : 주 연산은 맨 앞 요소에만 접근할 수 있다.

// [일반적인 큐 연산]
// Enqueue : 큐의 끝에 요소를 추가한다.
// Dequeue : 큐의 맨 앞 요소를 제거하고 반환한다.
// Peek : 큐의 맨 앞 요소를 제거하지 않고 반환한다.

// [사용 사례]
// 프로세스 스케줄링 : 운영체제에서 도착 순서에 따라 프로세스를 관리하기 위해 사용한다.
// 너비 우선 탐색(BFS) : 트리나 그래프 구조를 레벨별로 탐색하는데 사용된다.
// 비동기 통신 : 메시지가 수신된 순서에 따라 처리되어야 하는 시스템에 적합하다.

// [직접 구현해보기]
// 큐의 특징을 모두 가지도록 배열로 구현해보자.
public class MyQueue<T>
{
    private T[] _array;
    private int _head; // 현재 맨 앞에 있는 요소 위치
    private int _tail; // 현재 마지막 값이 있는 곳이 아닌 다음에 들어올 값을 저장할 위치
    private int _size; // 현재 요소 개수
    public int Count => _size;

    public MyQueue()
    {
        _array = Array.Empty<T>();
    }
    public MyQueue(int Capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
        _array = new T[Capacity];
    }

    // 저장 공간이 충분한지 검사
    // (만약에 공간이 없으면 배열을 늘린 후 값 복사)
    // 배열의 새로운 꼬리 위치 인덱스를 계산
    // 꼬리 위치에 값 할당
    public void Enqueue(T value)
    {
        if (_size == _array.Length)
        {
            ResizeArray();
        }
        _array[_tail] = value;

        // 다음 저장 위치로 이동한다.
        // 가득 찬 경우 저장 전에 확장하므로 기존 요소를 덮어쓰지 않는다.
        NextIndex(ref _tail);
        _size++;
    }

    // 꺼낼 요소가 1개 이상인지 확인
    // head위치의 값을 꺼내고 해당 위치 배열 기본값 초기화
    // 다음 헤드 위치 인덱스 계산후 갱신
    public T Dequeue()
    {
        if (_size < 1)
        {
            throw new InvalidOperationException("꺼낼 요소가 존재하지 않습니다.");
        }
        T outValue = _array[_head];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            _array[_head] = default!;
        }
        NextIndex(ref _head);
        _size--;
        return outValue;
    }

    // 확인할 요소가 1개 이상인지 확인
    // head위치의 값을 반환
    public T Peek()
    {
        if (_size < 1)
        {
            throw new InvalidOperationException("큐에 요소가 존재하지 않습니다.");
        }
        T outValue = _array[_head];
        return outValue;
    }

    private void NextIndex(ref int index)
    {
        int temp = index + 1;
        if (temp == _array.Length)
        {
            temp = 0;
        }
        index = temp;
    }

    // 현재 배열의 크기의 일정 배수만큼 늘린 크기를 계산
    // 최소 보정치와 최대 한계를 계산
    // 해당 크기만큼 배열크기 늘리고 값 복사
    private void ResizeArray()
    {
        const int GrowFactor = 2;
        const int MinimumGrow = 4;

        int newCapacity = _array.Length * GrowFactor;
        if ((uint)newCapacity > Array.MaxLength)
        {
            newCapacity = Array.MaxLength;
        }
        newCapacity = Math.Max(newCapacity, _array.Length + MinimumGrow);

        T[] newArray = new T[newCapacity];
        if (_head < _tail)
        {
            Array.Copy(_array, _head, newArray, 0, _size);
        }
        else
        {
            Array.Copy(_array, _head, newArray, 0, _array.Length - _head);
            Array.Copy(_array, 0, newArray, _array.Length - _head, _tail);
        }
        _array = newArray;
        _head = 0;
        _tail = _size;
    }
}