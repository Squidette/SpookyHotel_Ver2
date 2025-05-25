# C# 콘솔게임 프로젝트: The Spooky Hotel Ver.2
<br>
개인 프로젝트로, 유니티 사용 경험을 참고하여 C#에서 핵심 매니저, 씬, 오브젝트, 컴포넌트 클래스들이 존재하는 단순한 형태의 게임 엔진을 만들어보기 위한 노력<br>

플레이 영상: https://www.youtube.com/watch?v=7zs_OMVn0jo<br><br>


### 전체 구조<br>

![Image](https://github.com/user-attachments/assets/f0637533-56d9-4dcb-a88b-684c234c382d)<br>
사운드매니저, 씬매니저, 인풋매니저, 렌더러를 싱글톤으로 구성 후 씬에 게임오브젝트를 배치하여 게임 구성<br><br>


### .txt파일 로드와 콘솔 렌더러<br>

![Image](https://github.com/user-attachments/assets/7704f455-cb4a-445c-a441-7c4c3dced754)<br><br>
StreamReader를 사용하여 .txt로 만들어진 텍스트 아트의 가로세로 크기를 받아 로드할 수 있게 하고, 렌더러 클래스에서 콘솔에 그리게 함

```csharp
    /// <summary>
    /// 무엇을 어디에 그릴 것인지
    /// 입력한 캐프라이트의 중심점과 입력한 pos가 겹치는 위치에 그린다
    /// </summary>
    public void Draw(string spriteKey, CharSpriteCoords coords)
    {
        if (loadedCharSprites.TryGetValue(spriteKey, out CharSprite? cs))
        {
            CharSpriteCoords worldPoint = coords - cs.Center;

            for (int i = 0; i < cs.Size.col; i++)
            {
                for (int j = 0; j < cs.Size.row; j++)
                {
                    CharSpriteCoords offsetCoord = new CharSpriteCoords(worldPoint.col + i, worldPoint.row + j);

                    if (CharSpriteUtility.CoordsWithinBuffer(offsetCoord, canvasBuffer))
                    {
                        char? c = cs.GetCharByCoords(new CharSpriteCoords(i, j));

                        if (c != null)
                        {
                            canvasBuffer[offsetCoord.col, offsetCoord.row] = c.Value;
                        }
                    }
                }
            }

            bufferChange = true;
        }
    }
```

<br>

### 스프라이트 렌더러 컴포넌트

```csharp
// 이 오브젝트의 월드 좌표
CharSpriteCoords worldPosition = gameObject.Transform.GetWorldPosition();

// 현재 씬의 카메라 좌표 가져오기
CharSpriteCoords cameraPosition = SceneManager.Instance.CurrentScene.Camera.Transform.position;

// 렌더
ConsoleRenderer.Instance.Draw(charSpriteKey, worldPosition - cameraPosition);
```
오브젝트에 스프라이트 렌더러를 추가하고 스프라이트를 지정하면 해당 트랜스폼에 그리게 함<br><br>


### 씬매니저와 DontDestroyOnLoadScene 구현

씬매니저는 현재 씬 변수만 가진 채로 씬이 바뀔 때마다 해당 씬 클래스를 새로 인스턴스화하는 구조 (이 프로젝트처럼 가볍고 간단한 게임은 모든 씬을 한꺼번에 들고 있어도 상관없지만, 씬이 무거워진 일반적인 상황을 상정하면 이 방법이 낫지 않을까)

하지만 이렇게 하면 게임매니저 등 게임 전반에 존재하며 동작하는 게임오브젝트를 만들 수 없기 때문에 유니티처럼 DontDestroyOnLoad 씬 추가

```csharp
    public void FixedUpdate()
    {
        if (nextScene != null)
        {
            currentScene?.Exit();
            currentScene = nextScene;
            currentScene.Start();
            nextScene = null;
        }

        currentScene?.FixedUpdate();
        dontDestroyOnLoadScene.FixedUpdate();
    }
```
```csharp
    public void DontDestroyOnLoad(GameObject go)
    {
        // CurrentScene의 오브젝트를 DontDestroyOnLoadScene으로 옮긴다
        if (currentScene != null)
        {
            if (currentScene.RemoveGameObject(go))
            {
                go.ParentObject = null;
                dontDestroyOnLoadScene.AddGameObject(go);
            }
            else
            {
                Debug.Log("SceneManager_DontdestroyOnLoad[failed to remove gameObject from the original scene]");
            }
        }
        else
        {
            Debug.Log("SceneManager_DontdestroyOnLoad[currentScene null]");
        }
    }
```
![Image](https://github.com/user-attachments/assets/f1dd6b27-3c20-4735-9296-6b1c00b88241)<br>
DontDestroyOnLoad를 활용하여 만들어진 엘리베이터 (엘리베이터 오브젝트는 생성된 후 삭제되지 않고 씬의 전환과 상관없이 항상 작동)<br><br>


### Debug.Log 구현

```csharp
static class Debug
{
    static int currentLine = 0;

    // 버퍼 넘어가면 에러 떠서 줄 넘어가면 0줄부터 덮어쓰게 했다
    static public void Log(string message)
    {
        if (currentLine + 10 >= Console.BufferHeight - 1) currentLine = 0;
        Console.SetCursorPosition(0, currentLine + 10);
        Console.WriteLine(message);
        currentLine++;
    }
}
```

![Image](https://github.com/user-attachments/assets/0f10814c-35a8-4745-be4b-a963fca55a7e)<br>
간단한 스태틱 클래스로 전역에서 로그를 찍을 수 있게 함<br><br>


### 페이드사운드가 구현된 사운드매니저<br><br>

개별 사운드 클래스가 다음 네 상태 중 하나를 갖게 하여 끊김 없이 볼륨을 부드럽게 업데이트
```csharp
        public enum VolumeState
        {
            STATIC,                 // 볼륨 변화 없음
            INCREASING,             // 볼륨 증가
            DECREASING_TO_PAUSE,    // 정지를 위한 볼륨 감소
            DECREASING_TO_STOP      // 일시정지를 위한 볼륨 감소
        }
```

재생 중인 트랙들을 스태틱 리스트로 관리하며 주어진 상태에 따라 볼륨을 업데이트하게 함<br>
정지가 완료된 트랙들은 이 리스트에서 빠짐
```csharp
    // 트랙: 긴 파일을 반복재생, 자동 페이드 적용 (배경음용)
    class Track : Snippet
    {
        // 생성된 트랙 상태관리용 리스트
        static List<Track> tracks = new List<Track>();
        // ...
    }
```

<br>

### 무한의 방 배경 스크롤링 구현

![Image](https://github.com/user-attachments/assets/18701c21-671b-494f-8ece-bc7f04634323)<br>

3개의 동일한 배경 오브젝트를 배열로 저장하고, 현재 가장 왼쪽에 있는 오브젝트의 인덱스를 저장한 변수를 움직여 가며 추가적인 오브젝트 로드 없이 3개의 오브젝트를 굴려 가며 무한의 방 연출

```csharp
    // 쳇바퀴처럼 도는 3개의 배경 오브젝트 트랜스폼
    Transform[] slicedBackgrounds = null!;

    // 어떤 인덱스의 오브젝트가 가장 왼쪽에 있는지
    // 0 : 0 1 2
    // 1 : 1 2 0 <- 이 순서는 transform.position을 시각화한것
    // 2 : 2 0 1
    int slicesOrder = 0;
```

```csharp
    // 오른쪽으로 굴리기
    public void IncreaseSlicesOrder()
    {
        // 가장 오른쪽에 위치한 오브젝트의 인덱스
        int rightest = (slicesOrder + 2) % 3;

        // 맨 왼쪽에 있는걸 가장 오른쪽에있는애 오른쪽으로 옮기기
        //slicedBackgrounds[slicesOrder].position.row = slicedBackgrounds[rightest].position.row + distanceBetween;
        GetLeftPos() = GetRightPos() + distanceBetween;

        // 현 상태변수 증가시키기
        slicesOrder = (slicesOrder + 1) % 3;
    }
```

<br>

### :sweat: 이 프로젝트의 한계점

게임오브젝트 자체에 부모-자식 관계가 존재하나 씬이 게임오브젝트를 트리구조가 아닌 리스트로 가지고 있는 아쉬움
	(DontDestroyOnLoad 추가시에도 자식까지 딸려가지 못하기 때문에 불완전한 부분)

```csharp
class Scene
{
    protected List<GameObject> gameObjects;
    /// ...
}
```
