using System;
using System.Collections;
using System.Runtime.CompilerServices;

// 작성자 : 양사무엘
// 작성일자 : 2026-09-21
// 공부 주제 : 
// 목표 : 

class StudyHashTable
{
    static void Main()
    {
        Hashtable table = new Hashtable();
        HashSet<int> set = new HashSet<int>();
        Dictionary<int, int> dict = new Dictionary<int, int>();
    }
}

// [HashTable이란?]
// 키의 해시값으로 저장 위치를 계산하여, 데이터를 빠르게 저장하고 찾는 자료구조이다.

//[핵심]
// 해시 함수 : 입력(키)을 받아 해시값으로 변환하는 작업을 수행한다. 목표는 빠르게 계산하면서 키들이 특정 값에 몰리지 않도록 분산시켜야 하는 것이다.
// 버킷(Bucket): 해시값을 바탕으로 선택하는 저장·검색의 기준 구역이다. 이번 구현에서는 버킷 배열의 각 칸이 해당 버킷에 연결된 첫 항목의 인덱스를 저장한다.
// 항목(Entry) : 실제 저장되는 데이터 한 건이다. 이번 키–값 해시 테이블에서는 키와 값을 저장하며, 해시 코드와 다음 항목 인덱스 같은 관리 정보도 포함한다.
// 충돌 처리 : 서로 다른 키가 같은 버킷으로 배정되는 현상이다. 해시값 자체가 같거나, 해시값이 달라도 버킷 인덱스 계산 결과가 같으면 발생할 수 있다.

// [특징]
// 해싱 기반 접근 
// 키를 통한 데이터 구분
// 충돌 처리 필요
// 부하율에 따른 성능 변화
// 추가 메모리 사용 : 데이터 외에도 버킷, 연결 정보, 비어 있는 저장 공간 등이 필요하다. 사용하는 충돌 처리 방식과 용량 정책에 따라 메모리 사용량이 달라진다.

// [해시 함수]
// 해시 함수에는 여러 종류가 있다. 해시값을 계산하는 방법과 해시값으로 버킷의 주소를 계산하는 방법이 있다. 또 이 둘을 같이 처리하는 방법도 있다.
// 대표적으로 나눗셈법, 곱셈법, 접지법 등이 있다.

// [좋은 해시 함수의 특징]
// 해시 값 충돌의 최소화
// 해시 테이블 전체에 해시 값이 균일하게 분포
// 쉽고 빠른 연산

// [충돌 처리]
// 충돌 처리 방법에는 대표적으로 두가지 방법이 있다. 체이닝방식과 개방 주소 방식이 있다.
// 체이닝은 충돌한 해시에 대한 항목들을 같은 버킷에 연결지어서 관리하는 방식이다.
// 개방 주소 방식은 충돌한 해시에 대해 다음 저장 위치를 탐사하여 저장 및 검색하는 방식이다. 탐사 방법에는 선형 탐사, 제곱 탐사, 이중해싱이 있다.

// [LoadFactor(부하율)]
// 부하율은 현재 저장된 항목 수를 버킷 수로 나눈 값이다.
// 체이닝에서는 버킷당 평균 항목 수를, 한 칸에 한 항목을 저장하는 개방 주소법에서는 전체 슬롯 중 사용 중인 슬롯의 비율을 나타낸다.
// 부하율이 높아지면 검색 비용이 증가할 수 있으며, 확장 기준은 구현마다 다르다.
// 버킷 수를 늘리고 기존 항목을 재배치하면 부하율을 낮출 수 있다.

// [HashTable 기본 연산]
// 삽입 : 해시 함수를 사용하여 키의 해시값을 계산 한 후 해시값으로 시작 위치를 계산하고, 충돌 처리 규칙에 따라 항목을 저장한다.
// 검색 : 키의 해시값으로 시작 위치를 계산한 뒤, 충돌 처리 규칙에 따라 후보 항목을 탐색하고 키를 비교한다.
// 삭제 : 키에 해당하는 항목을 찾아 제거하고, 다른 항목의 검색이 유지되도록 연결이나 슬롯 상태를 갱신한다.

// [직접 구현해보기]
// 버킷, 충돌 처리를 구현하고 내부적으로 배열방식을 사용하여 항목을 저장해보자.
// .Net이 제공하는 HashTable이 아닌 Dictionary쪽에 더 가까운 방식으로 구현해보자.
public class MyHashTable<TKey, TValue>
{
    private const int NoneElement = -1;
    private int[] _buckets; // 같은 버킷 인덱스로 계산된 항목들의 첫 항목 인덱스
    private Entry[] _entries; // 항목이 저장되는 배열
    private MyStack<int> _freeList;
    private int _entriesUsedEnd;
    private int _elementCount; // 현재 저장된 키값 쌍의 개수

    public MyHashTable()
    {
        _buckets = Array.Empty<int>();
        _entries = Array.Empty<Entry>();
        _freeList = new MyStack<int>();
    }

    // 키의 값이 안전한지 검사(Null검사) 후 키의 값으로 해시코드를 가져옴
    // 내부에 중복된 키 값이 있다면 throw
    // 요소를 저장할 공간이 있는지 확인
    // _entries의 빈공간 가져오고 해당 위치에 저장
    // 해시코드로 저장할 버킷의 위치를 가져오고 항목배열 인덱스 저장
    public void Add(TKey key, TValue value)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (ContainsKey(key)) throw new InvalidOperationException();

        int hashCode = key.GetHashCode();

        if (_buckets.Length == 0 || _elementCount == _entries.Length)
        {
            ArrayResize();
        }

        Entry entry = new Entry(hashCode, key, value);

        int entryIndex = GetEmptyEntries();

        int bucketIndex = GetBucketIndex(entry.hashCode, _buckets.Length);
        entry.next = _buckets[bucketIndex];
        _entries[entryIndex] = entry;
        _buckets[bucketIndex] = entryIndex;
        _elementCount++;
    }

    // 키의 값이 안전한지 검사 후 키값으로 해시코드를 가져옴
    // 해시코드를 가지고 저장 위치를 계산하여 첫 요소에 접근함
    // 버킷에 요소가 없으면 그대로 throw
    // 버킷의 요소들의 Key와 대조하며 비교 (이때 다음 요소가 -1(없으면) throw)
    // 찾은 요소를 제거하고 참조면 항목배열에서 기본값 초기화, freeList에 해당 항목배열 인덱스 추가, 버킷과 연결리스트인덱스 이어주기
    // _elementCount 줄이기
    public void Remove(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (_elementCount == 0)
            throw new KeyNotFoundException();

        int hashCode = key.GetHashCode();
        int bucketIndex = GetBucketIndex(hashCode, _buckets.Length);
        ref int nextPoint = ref _buckets[bucketIndex];

        while (true)
        {
            if (nextPoint == NoneElement) throw new KeyNotFoundException();
            if (_entries[nextPoint].key!.Equals(key))
            {
                _freeList.Push(nextPoint);
                int saveCurPoint = nextPoint;
                nextPoint = _entries[nextPoint].next;
                if (RuntimeHelpers.IsReferenceOrContainsReferences<Entry>())
                {
                    _entries[saveCurPoint] = default;
                }
                break;
            }
            nextPoint = ref _entries[nextPoint].next;
        }
        _elementCount--;
    }

    public bool ContainsKey(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (_elementCount == 0)
            return false;

        int hashCode = key.GetHashCode();
        int bucketIndex = GetBucketIndex(hashCode, _buckets.Length);
        int nextPoint = _buckets[bucketIndex];

        while (true)
        {
            if (nextPoint == NoneElement) return false;
            if (_entries[nextPoint].key!.Equals(key))
            {
                return true;
            }
            nextPoint = _entries[nextPoint].next;
        }
    }

    // 키가 유효한지 검사하기(null검사), 검사할 요소가 존재하는지 확인
    // 키를 해시코드로 변환하고 해시코드를 기반으로 항목이 존재할 버킷 위치를 계산
    // 버킷에서 항목 요소 검사하고 반환하기
    public bool TryGetValue(TKey key, out TValue value)
    {
        value = default!;

        if (key is null) return false;

        if (_elementCount == 0)
            return false;

        int hashCode = key.GetHashCode();
        int bucketIndex = GetBucketIndex(hashCode, _buckets.Length);
        int nextPoint = _buckets[bucketIndex];

        while (true)
        {
            if (nextPoint == NoneElement) return false;
            if (_entries[nextPoint].key!.Equals(key))
            {
                value = _entries[nextPoint].value;
                return true;
            }
            nextPoint = _entries[nextPoint].next;
        }
    }

    private void ArrayResize()
    {
        const int SizeFactor = 2;
        const int MinSize = 4;

        int newSize = _entries.Length * SizeFactor;
        if ((uint)newSize > Array.MaxLength) newSize = Array.MaxLength;
        newSize = Math.Max(newSize, MinSize);

        Entry[] newEntries = new Entry[newSize];
        Array.Copy(_entries, newEntries, _elementCount);
        _entries = newEntries;

        int[] newBuckets = new int[newSize];
        Array.Fill(newBuckets, NoneElement);

        for (int i = 0; i < _elementCount; i++)
        {
            int BucketIndex = GetBucketIndex(_entries[i].hashCode, newBuckets.Length);
            _entries[i].next = newBuckets[BucketIndex];
            newBuckets[BucketIndex] = i;
        }
        _buckets = newBuckets;
    }

    // 해시값을 지정된 버킷 수 범위의 인덱스로 변환한다
    private int GetBucketIndex(int hashCode, int bucketLength)
    {
        return (int)((uint)hashCode % (uint)bucketLength);
    }

    private int GetEmptyEntries()
    {
        if (_freeList.TryPop(out int index))
        {
            return index;
        }
        // 앞서 공간이 부족하면 배열을 재생성하기 때문에 마지막빈공간인덱스가 배열의 최대 크기를 벗어날 일은 없음
        return _entriesUsedEnd++;
    }

    private struct Entry
    {
        public int hashCode;
        public int next;
        public TKey key;
        public TValue value;

        public Entry(int HashCode, TKey Key, TValue Value)
        {
            hashCode = HashCode;
            key = Key;
            value = Value;
        }
    }
}