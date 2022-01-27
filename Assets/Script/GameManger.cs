using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManger : MonoBehaviour {
	public static bool w_color = true; // true = black , false = white
	public int[] chese_data = new int[81];
	public Renderer[] floor = new Renderer[81];

	private int temp_id;
	private int temp_color;

	public static bool wingame;
	private int cheseCount;

	public GameObject re_btn;

	public GameObject[] image_ChassIcon = new GameObject[2];
	public Image[] image_ChassText = new Image[2];
	public Image[] image_Line = new Image[2];
	public Text[] timeColor = new Text[2];
	public GameObject image_WinIcon;

	float timmer; //秒數倒數
	bool useTime; //是否開始倒數

	void Awake()
	{
		cheseCount = 0;
		wingame = false;
		w_color = true;

		timmer = 31f;
		useTime = true;
	}

	void Update()
	{
		if (useTime == true)
		{
			timmer -= Time.deltaTime;
			TimeControl ();
		}
	}

	void GetID(int id)
	{
		temp_id = id;
	}

	void GetColor(int bg)
	{
		temp_color = bg;
	}

	void WhoTurn()
	{
		timmer = 31f;
		cheseCount++;
		chese_data [temp_id] = temp_color;
		CheckLine ();
	}
		
	void CheckLine() 
	{
		if (!wingame)
			UpDown ();
		else {
			return;
		}
			
		if (!wingame)
			LeftRight ();
		else{
			return;
		}

		if (!wingame)
			LeftTopRightDown();
		else{
			return;
		}

		if (!wingame)
			RightTopLeftDown ();
		else{
			return;
		}

		//--------------------------------------------------UI部分
		if ((cheseCount % 2) == 0) // 黑子下
		{ 
			image_ChassIcon [1].SetActive(false);
			image_ChassIcon [0].SetActive(true);

			image_ChassText [0].color = Color.black;
			image_ChassText [1].color = Color.white;
			image_Line[0].color = Color.black;
			image_Line[1].color = Color.white;
			timeColor[0].color = Color.black;
			timeColor[1].color = Color.white;
		} 
		else if ((cheseCount % 2) == 1) // 白子下
		{
			image_ChassIcon [0].SetActive(false);
			image_ChassIcon [1].SetActive(true);

			image_ChassText [1].color = Color.black;
			image_ChassText [0].color = Color.white;
			image_Line[1].color = Color.black;
			image_Line[0].color = Color.white;
			timeColor[1].color = Color.black;
			timeColor[0].color = Color.white;
		}

		if (cheseCount == 81 && !wingame) {
			wingame = true;
			EndGame ();
		} else {
			w_color = !w_color; //判斷完是否連線之後，交換顏色
		}
    }

    //上下:9
    void UpDown()
	{
		int[] linkarr = new int[9]{ temp_id, -1, -1, -1, -1, -1, -1, -1, -1 };
		int count = 1;

		int nextN = temp_id - 9;

		//上
		if (nextN > -1)
		{
			for (int i = temp_id; (i - 9) > -1; i -= 9)
			{
				if (chese_data [temp_id] == chese_data [nextN])
				{
					//print ("Top - same");
					linkarr [count] = nextN;
					count++;
					nextN -= 9;
				}
				else
				{
					//print ("Top - different");
					break;
				}
			}
		}

		nextN = temp_id + 9;

		//下
		if (nextN < 81)
		{
			for (int i = temp_id; (i + 9) < 81; i += 9)
			{
				if (chese_data [temp_id] == chese_data [nextN])
				{
					//print ("Down - same");
					linkarr [count] = nextN;
					count++;
					nextN += 9;
				}
				else
				{
					//print ("Down - different");
					break;
				}
			}
		}

		//結算
		if (count >= 5) {
			print ("win: " + chese_data [temp_id]);
			for (int i = 0; i < linkarr.Length; i++) {
				if (linkarr [i] != -1)
					floor [linkarr [i]].material.color = Color.red;
				else
					break;
			}
			wingame = true;
			EndGame ();
		}
	}

    //左右:1 
    void LeftRight()
    {
        int[] linkarr = new int[9] { temp_id, -1, -1, -1, -1, -1, -1, -1, -1 };
        int count = 1;

        int nextN = temp_id - 1;

        //左
		if (!LR_Run(temp_id))
        {
			for (int i = temp_id; !LR_Run(i) ; i -= 1)
            {
                if (chese_data[temp_id] == chese_data[nextN])
                {
                    //print("Left - same");
                    linkarr[count] = nextN;
                    count++;
                    nextN -= 1;
                }
                else
                {
                    //print("Left - different");
                    break;
                }
            }
        }

        nextN = temp_id + 1;

        //右
		if (!LR_Run(temp_id))
        {
			for (int i = temp_id; !LR_Run(i); i += 1)
            {
                if (chese_data[temp_id] == chese_data[nextN])
                {
                    //print("Right - same");
                    linkarr[count] = nextN;
                    count++;
                    nextN += 1;
                }
                else
                {
                    //print("Right - different");
                    break;
                }
            }
        }

        //結算
        if (count >= 5)
        {
            print("win: " + chese_data[temp_id]);
            for (int i = 0; i < linkarr.Length; i++)
            {
                if (linkarr[i] != -1)
                    floor[linkarr[i]].material.color = Color.red;
                else
                    break;
            }
            wingame = true;
			EndGame ();
        }
    }

    //左上右下:10 
    void LeftTopRightDown()
    {
        int[] linkarr = new int[9] { temp_id, -1, -1, -1, -1, -1, -1, -1, -1 };
        int count = 1;

        int nextN = temp_id - 10;

        //左上
        if(!LT_Run(temp_id))
        {
            for (int i = temp_id; !LT_Run(i); i -= 10)
            {
                if (chese_data[temp_id] == chese_data[nextN])
                {
                    //print("LeftTop - same");
                    linkarr[count] = nextN;
                    count++;
                    nextN -= 10;
                }
                else
                {
                    //print("LeftTop - different");
                    break;
                }
            }
        }

        nextN = temp_id + 10;

        //右下
        if (!RD_Run(temp_id))
        {
            for (int i = temp_id; !RD_Run(i); i += 10)
            {
                if (chese_data[temp_id] == chese_data[nextN])
                {
                    //print("RightDown - same");
                    linkarr[count] = nextN;
                    count++;
                    nextN += 10;
                }
                else
                {
                    //print("RightDown - different");
                    break;
                }
            }
        }

        //結算
        if (count >= 5)
        {
            print("win: " + chese_data[temp_id]);
            for (int i = 0; i < linkarr.Length; i++)
            {
                if (linkarr[i] != -1)
                    floor[linkarr[i]].material.color = Color.red;
                else
                    break;
            }
            wingame = true;
			EndGame ();
        }
    }

    //右上左下:8
    void RightTopLeftDown()
    {
        int[] linkarr = new int[9] { temp_id, -1, -1, -1, -1, -1, -1, -1, -1 };
        int count = 1;

        int nextN = temp_id - 8;

        //右上
        if (!RT_Run(temp_id))
        {
            for (int i = temp_id; !RT_Run(i); i -= 8)
            {
                if (chese_data[temp_id] == chese_data[nextN])
                {
                    //print("RightTop - same");
                    linkarr[count] = nextN;
                    count++;
                    nextN -= 8;
                }
                else
                {
                    //print("RightTop - different");
                    break;
                }
            }
        }

        nextN = temp_id + 8;

        //左下
        if (!LD_Run(temp_id))
        {
            for (int i = temp_id; !LD_Run(i); i += 8)
            {
                if (chese_data[temp_id] == chese_data[nextN])
                {
                    //print("RightDown - same");
                    linkarr[count] = nextN;
                    count++;
                    nextN += 8;
                }
                else
                {
                    //print("RightDown - different");
                    break;
                }
            }
        }

        //結算
        if (count >= 5)
        {
            print("win: " + chese_data[temp_id]);
            for (int i = 0; i < linkarr.Length; i++)
            {
                if (linkarr[i] != -1)
                    floor[linkarr[i]].material.color = Color.red;
                else
                    break;
            }
            wingame = true;
			EndGame ();
        }
    }

	void EndGame()
	{
		useTime = false;
		if (wingame == true) {
			image_WinIcon.SetActive(true);
		}
			
		re_btn.SetActive (true);
	}

	public void LoadNewGame()
	{
		SceneManager.LoadScene (0);
	}


    //==================================================
	bool LR_Run(int nownum)
	{
		int[] outNum = new int[18] {0, 9, 18, 27, 36, 45, 54, 63, 72, 8, 17, 26, 35, 44, 53, 62, 71, 80};
		bool tempX = false;

		for (int i = 0; i < outNum.Length; i++)
		{
			if (nownum == outNum[i])
			{
				tempX = true;
				break;
			}
		}
		return tempX;
	}

    bool LT_Run(int nownum)
    {
        int[] outNum = new int[17] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 27, 36, 45, 54, 63, 72 };
        bool tempX = false;

        for (int i = 0; i < outNum.Length; i++)
        {
            if (nownum == outNum[i])
            {
                tempX = true;
                break;
            }
        }
        return tempX;
    }

    bool RD_Run(int nownum)
    {
        int[] outNum = new int[17] { 8, 17, 26, 35, 44, 53, 62, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80};
        bool tempX = false;

        for (int i = 0; i < outNum.Length; i++)
        {
            if (nownum == outNum[i])
            {
                tempX = true;
                break;
            }
        }
        return tempX;
    }

    bool RT_Run(int nownum)
    {
        int[] outNum = new int[17] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 17, 26, 35, 44, 53, 62, 71, 80 };
        bool tempX = false;

        for (int i = 0; i < outNum.Length; i++)
        {
            if (nownum == outNum[i])
            {
                tempX = true;
                break;
            }
        }
        return tempX;
    }

    bool LD_Run(int nownum)
    {
        int[] outNum = new int[17] { 0, 9, 18, 27, 36, 45, 54, 63, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
        bool tempX = false;

        for (int i = 0; i < outNum.Length; i++)
        {
            if (nownum == outNum[i])
            {
                tempX = true;
                break;
            }
        }
        return tempX;
    }


	//====================================== 時間倒數
	void TimeControl()
	{
		if (timmer <= 0) {
			useTime = false;
			//設定另一方贏
			image_ChassIcon [cheseCount%2].SetActive(false);
			image_ChassIcon [(cheseCount+1)%2].SetActive(true);

			image_ChassText [(cheseCount+1)%2].color = Color.black;
			image_ChassText [cheseCount%2].color = Color.white;
			image_Line[(cheseCount+1)%2].color = Color.black;
			image_Line[cheseCount%2].color = Color.white;
			timeColor[(cheseCount+1)%2].color = Color.black;
			timeColor[cheseCount%2].color = Color.white;
			wingame = true;
			EndGame ();
		}
		TimeText ();
	}

	void TimeText()
	{
		int m = (int)timmer / 60;
		int s = (int)timmer % 60;

		timeColor [cheseCount%2].text = m.ToString ("0") + ":" + s.ToString ("00");
	}
}


        