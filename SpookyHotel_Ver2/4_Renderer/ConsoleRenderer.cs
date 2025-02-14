/// <summary>
/// 텍스트를 콘솔창에 출력해주는 클래스
/// </summary>
class ConsoleRenderer
{
    // 싱글턴
    static ConsoleRenderer instance = null!;

    public static ConsoleRenderer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConsoleRenderer();
            }
            return instance;
        }
    }

    // 모든 것이 합쳐질 도화지 버퍼
    char[,] canvasBuffer;

    // 버퍼가 바뀔 때만 렌더하기 위해 매 FixedUpdate에서 관리되는 불변수
    bool bufferChange;

    // .txt파일당 한번씩만 로드하도록 관리
    Dictionary<string, CharSprite> loadedCharSprites;

    ConsoleRenderer()
    {
        canvasBuffer = new char[10, 30];
        bufferChange = true;
        loadedCharSprites = new Dictionary<string, CharSprite>();

        // 커서 안보이게 하기
        Console.CursorVisible = false;

        // test - 초기화
        for (int i = 0; i < canvasBuffer.GetLength(0); i++)
        {
            for (int j = 0; j < canvasBuffer.GetLength(1); j++)
            {
                canvasBuffer[i, j] = 'x';
            }
        }
    }

    ~ConsoleRenderer()
    {
        loadedCharSprites.Clear();
    }

    // 버퍼에 입력
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

    public void LoadSprite(string spriteKey, CharSpriteSize size, CharSpriteCoords center, string? fileName = null, bool transparent = true)
    {
        if (!loadedCharSprites.ContainsKey(spriteKey))
        {
            CharSprite newCharSprite = new CharSprite(size, center, fileName, transparent);
            loadedCharSprites.Add(spriteKey, newCharSprite);
        }
    }

    public void UnloadSprite(string spriteKey)
    {
        if (loadedCharSprites.ContainsKey(spriteKey))
        {
            loadedCharSprites.Remove(spriteKey);
        }
    }

    public CharSprite? GetSprite(string spriteKey)
    {
        if (loadedCharSprites.ContainsKey(spriteKey))
        {
            return loadedCharSprites[spriteKey];
        }
        else return null;
    }

    // 버퍼를 출력한다
    public void Render()
    {
        if (bufferChange)
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.Write(canvasBuffer[i, j]);
                }
                Console.WriteLine();
            }
            bufferChange = false;
        }
    }
}

/// <summary>
/// 이 콘솔프로젝트의 스프라이트에 해당하는 클래스 aka 캐프라이트
/// char의 2차원 배열과 중심점을 저장
/// 생성시 파일이름을 지정하면 .txt파일을 읽어온다
/// </summary>
class CharSprite
{
    // 내용물(모양)
    public char[,] buffer;

    // 가로세로 크기
    CharSpriteSize size;
    public CharSpriteSize Size
    {
        get { return size; }
    }

    // 중심점
    CharSpriteCoords center;
    public CharSpriteCoords Center
    {
        get { return center; }
    }
    
    // 반투명인지? (' '를 투명취급할 것인지 여부)
    bool transparent;

    // 상대경로
    static string resourcesPath = "..\\..\\..\\..\\Resources\\CharSprites\\";

    public CharSprite(CharSpriteSize size, CharSpriteCoords center, string? fileName = null, bool transparent = true)
    {
        this.size = size;
        this.center = center;
        this.transparent = transparent;

        // 버퍼 채우기
        buffer = new char[size.col, size.row];

        bool? checkFail = null;

        // 파일명을 입력했다면 읽어온다
        if (fileName != null)
        {
            checkFail = false;

            using (StreamReader sr = new StreamReader(resourcesPath + fileName))
            {
                for (int i = 0; i < size.col; i++)
                {
                    string? line = sr.ReadLine();

                    if (line == null)
                    {
                        checkFail = true;
                        break;
                    }

                    for (int j = 0; j < size.row; j++)
                    {
                        buffer[i, j] = j < line.Length ? line[j] : ' ';
                    }
                }
            }
        }

        // 파일명을 입력하지 않았거나 읽다가 실패했으면 빈 캐프라이트로 초기화
        if (checkFail == null || checkFail == true)
        {
            DrawBlankBuffer();
        }
    }

    void DrawBlankBuffer()
    {
        for (int i = 0; i < buffer.GetLength(0); i++)
        {
            for (int j = 0; j < buffer.GetLength(1); j++)
            {
                buffer[i, j] = ' ';
            }
        }
    }

    public char? GetCharByCoords(CharSpriteCoords coords)
    {
        if (CharSpriteUtility.CoordsWithinBuffer(coords, buffer))
        {
            char c = buffer[coords.col, coords.row];
            if (transparent && c == ' ') return null;
            else return c;
        }
        else return null;
    }

    public void SetCharByCoords(CharSpriteCoords coords, char newChar)
    {
        if (CharSpriteUtility.CoordsWithinBuffer(coords, buffer))
        {
            buffer[coords.col, coords.row] = newChar;
        }
    }
}

public static class CharSpriteUtility
{
    /// <summary>
    /// 버퍼 크기 내의 좌표인가?
    /// </summary>
    public static bool CoordsWithinBuffer(CharSpriteCoords coords, char[,] buffer)
    {
        bool within = true;

        if (coords.col < 0 || coords.col >= buffer.GetLength(0)) within = false;
        if (coords.row < 0 || coords.row >= buffer.GetLength(1)) within = false;

        return within;
    } 
}

/// <summary>
/// 좌표용
/// </summary>
public struct CharSpriteCoords
{
    public int col;
    public int row;

    public CharSpriteCoords(int column, int row)
    {
        col = column;
        this.row = row;
    }

    public static CharSpriteCoords operator +(CharSpriteCoords a, CharSpriteCoords b)
    {
        return new CharSpriteCoords(a.col + b.col, a.row + b.row);
    }

    public static CharSpriteCoords operator -(CharSpriteCoords a, CharSpriteCoords b)
    {
        return new CharSpriteCoords(a.col - b.col, a.row - b.row);
    }

    public override string ToString()
    {
        return $"[{col}, {row}]";
    }
}

/// <summary>
/// 캐프라이트의 크기 전용 구조체
/// 최소 사이즈 1x1
/// </summary>
public struct CharSpriteSize
{
    public int col;
    public int row;

    public CharSpriteSize(int column, int row)
    {
        col = column < 1 ? 1 : column;
        this.row = row < 1 ? 1 : row;
    }
}