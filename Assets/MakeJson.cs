using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JsonDatas;
using Newtonsoft.Json;
using static UnityEngine.GraphicsBuffer;
using CustomUtils;
using UnityEditor;


public class HsJsonData
{
    public CameraData camera_data = new CameraData();
    public ObjectData[] objects;
}

[System.Serializable]
public class ObjectData
{
    public string _class = "";
    public object local_cuboid = null;
    public float[][] local_to_world_matrix;
    public float[] location = new float[3];
    public float[] location_worldframe = new float[3];
    public string name = "";
    public float[][] projected_cuboid;
    public float[] quaternion_xyzw = new float[4];
    public float[] quaternion_xyzw_worldframe = new float[4];
    public int segmentation_id = 1;
    public float visibility = 0;

}

[System.Serializable]
public class CameraData
{
    public camera_look_at camera_look_at = new camera_look_at();
    public float[][] camera_view_matrix;
    public int height;
    public intrinsics intrinsics = new intrinsics();
    public float[] location_worldframe = new float[3];
    public float[] quaternion_xyzw_worldframe = new float[4];
    public int width;


}

public class MakeJson : MonoBehaviour
{
    public Vector3 MinBoundary;
    public Vector3 MaxBoundary;

    public List<GameObject> objects = new List<GameObject>();
    public static Camera JsonCam;
    public GameObject PointObj;
    string path;
    CustomUtils.ScreenShot screenShot;
    void OnEnable() => screenShot = this.GetComponent<ScreenShot>();

    void Awake()
    {
        JsonCam = this.GetComponent<Camera>();
        if (JsonCam == null)
        {
            Debug.LogError("촬영 카메라가 할당되지 않았습니다");
        }

        string path = "c:\\Users\\user\\Desktop\\capstone";

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("폴더 경로가 잘못되었습니다");
            return;
        }

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    void Start()
    {
        Matrix4x4 projectionMatrix = JsonCam.projectionMatrix;
        Debug.Log(projectionMatrix);

    }

    public void JsonSave(int indexnumber,string Jsonpath)
    {
        HsJsonData jsonData = new HsJsonData();

        #region 카메라 데이터 값 넣기
        // camera_look_at -> at 값 넣기
        jsonData.camera_data.camera_look_at.at[0] = JsonCam.transform.forward.x;
        jsonData.camera_data.camera_look_at.at[1] = JsonCam.transform.forward.y;
        jsonData.camera_data.camera_look_at.at[2] = JsonCam.transform.forward.z;

        // camera_look_at -> eye 값 넣기
        jsonData.camera_data.camera_look_at.eye[0] = JsonCam.transform.position.x;
        jsonData.camera_data.camera_look_at.eye[1] = JsonCam.transform.position.y;
        jsonData.camera_data.camera_look_at.eye[2] = JsonCam.transform.position.z;

        // camera_look_at -> up 값 넣기
        jsonData.camera_data.camera_look_at.up[0] = JsonCam.transform.up.x;
        jsonData.camera_data.camera_look_at.up[1] = JsonCam.transform.up.y;
        jsonData.camera_data.camera_look_at.up[2] = JsonCam.transform.up.z;

        //camera_view_matrix 값 넣기
        Matrix4x4 viewMat = JsonCam.worldToCameraMatrix;

        jsonData.camera_data.camera_view_matrix = new float[][]
        {
            new float[]{viewMat[0,0],viewMat[0,1],viewMat[0,2],viewMat[0,3]},
            new float[]{viewMat[1,0],viewMat[1,1],viewMat[1,2],viewMat[1,3]},
            new float[]{viewMat[2,0],viewMat[2,1],viewMat[2,2],viewMat[2,3]},
            new float[]{viewMat[3,0],viewMat[3,1],viewMat[3,2],viewMat[3,3]}
        };

        //height 값 넣기
        jsonData.camera_data.height = 500;

        //intrinsics 값 넣기
        // 센서 사이즈 (가로, 세로)
        float sensorWidth = JsonCam.sensorSize.x;
        float sensorHeight = JsonCam.sensorSize.y;

        // 픽셀 단위의 초점거리 (가로, 세로)
        float focalLengthX = JsonCam.focalLength;
        float focalLengthY = JsonCam.focalLength;

        Matrix4x4 projectionMatrix = JsonCam.projectionMatrix;

        // Get camera parameters
        float _width = JsonCam.pixelWidth;
        float _height = JsonCam.pixelHeight;

        float verticalFov = JsonCam.fieldOfView;
        float aspectRatio = (float)_width / _height;

        // Calculate horizontal FOV (in radians)
        float horizontalFov = 2f * Mathf.Atan(Mathf.Tan(verticalFov * Mathf.Deg2Rad / 2f) * aspectRatio);


        /*
        Debug.Log($"Screen_Width : {_width} , Screen_Height : {_height} " +
            $", HorizontalFov : {horizontalFov} , VerticalFov : {verticalFov}");
        */
         
        // Calculate fx and fy
        float cx = (_width / 2);
        float cy = (_height / 2);
        float fx = cx / (Mathf.Tan(verticalFov * Mathf.Deg2Rad / 2f));
        float fy = cy / (Mathf.Tan(verticalFov * Mathf.Deg2Rad / 2f));

        jsonData.camera_data.intrinsics.cx = (_width / 2);
        jsonData.camera_data.intrinsics.cy = (_height / 2);
        jsonData.camera_data.intrinsics.fx = fx;
        jsonData.camera_data.intrinsics.fy = fy;

        // location_worldframe 값 넣기
        jsonData.camera_data.location_worldframe[0] = JsonCam.transform.position.x;
        jsonData.camera_data.location_worldframe[1] = JsonCam.transform.position.y;
        jsonData.camera_data.location_worldframe[2] = JsonCam.transform.position.z;

        // quaternion_xyzw_worldframe 값 넣기
        jsonData.camera_data.quaternion_xyzw_worldframe[0] = JsonCam.transform.rotation.x;
        jsonData.camera_data.quaternion_xyzw_worldframe[1] = JsonCam.transform.rotation.y;
        jsonData.camera_data.quaternion_xyzw_worldframe[2] = JsonCam.transform.rotation.z;
        jsonData.camera_data.quaternion_xyzw_worldframe[3] = JsonCam.transform.rotation.w;

        // width 값넣기
        jsonData.camera_data.width = 500;
        #endregion

        #region 물체 데이터 값 넣기
        //화면상 물체 수 만큼 objectData 동적할당
        jsonData.objects = new ObjectData[objects.Count];

        for (int i = 0; i < jsonData.objects.Length; i++) 
        {
            jsonData.objects[i] = new ObjectData();

            
            // bounding_box_minx_maxx_miny_maxy 값 채우기
            BoxCollider collider = objects[i].GetComponent<BoxCollider>();
            if (collider == null)
            {
                Debug.LogError("Collider가 없습니다.");
                return;
            }
           


            // _class 값 채우기
            jsonData.objects[i]._class = "bar";


            // local_to_world_matrix 값 채우기
            Matrix4x4 localToWorldMatrix = objects[i].transform.localToWorldMatrix;
            jsonData.objects[i].local_to_world_matrix
                = new float[][]{
                new float[] 
                {
                    localToWorldMatrix[0,0], localToWorldMatrix[0, 1],
                    localToWorldMatrix[0,2], localToWorldMatrix[0, 3]
                },
                new float[]
                {
                    localToWorldMatrix[1,0], localToWorldMatrix[1, 1],
                    localToWorldMatrix[1,2], localToWorldMatrix[1, 3]
                },
                new float[]
                {
                    localToWorldMatrix[2,0], localToWorldMatrix[2, 1],
                    localToWorldMatrix[2,2], localToWorldMatrix[2, 3]
                },
                new float[]
                {
                    localToWorldMatrix[3,0], localToWorldMatrix[3, 1],
                    localToWorldMatrix[3,2], localToWorldMatrix[3, 3]
                }

            };

            // location 값 채우기 -- 카메라 좌표계에서 객체 위치
            jsonData.objects[i].location[0]
                = JsonCam.WorldToViewportPoint (objects[i].transform.localPosition).x;
            jsonData.objects[i].location[1]
                = 1 - JsonCam.WorldToViewportPoint(objects[i].transform.localPosition).y;
            jsonData.objects[i].location[2]
                = JsonCam.WorldToViewportPoint(objects[i].transform.localPosition).z;


            // location_worldframe 값 채우기
            jsonData.objects[i].location_worldframe[0]
                = objects[i].transform.position.x;
            jsonData.objects[i].location_worldframe[1]
                = objects[i].transform.position.y;
            jsonData.objects[i].location_worldframe[2]
                = objects[i].transform.position.z;

            // name 채우기
            jsonData.objects[i].name = objects[i].name;

            //quaternion_xyzw 채우기 -- 카메라 좌표계 에서의 객체 회전값
            jsonData.objects[i].quaternion_xyzw[0]
                = (JsonCam.transform.rotation * objects[i].transform.rotation).x;
            jsonData.objects[i].quaternion_xyzw[1]
                = (JsonCam.transform.rotation * objects[i].transform.rotation).y;
            jsonData.objects[i].quaternion_xyzw[2]
                = (JsonCam.transform.rotation * objects[i].transform.rotation).z;
            jsonData.objects[i].quaternion_xyzw[3]
                = (JsonCam.transform.rotation * objects[i].transform.rotation).w;

            // quaternion_xyzw_worldframe
            jsonData.objects[i].quaternion_xyzw_worldframe[0]
                = objects[i].transform.rotation.x;
            jsonData.objects[i].quaternion_xyzw_worldframe[1]
                = objects[i].transform.rotation.y;
            jsonData.objects[i].quaternion_xyzw_worldframe[2]
                = objects[i].transform.rotation.z;
            jsonData.objects[i].quaternion_xyzw_worldframe[3]
                = objects[i].transform.rotation.w;

            // segmentation_id 채우기
            jsonData.objects[i].segmentation_id = i;

            // visibility 채우기
            jsonData.objects[i].visibility = 1;

            // projected_cuboid 채우기 및 포인트 점 생성 -- 실제 값과 다름
            Vector3 size = Vector3.Scale(collider.size,
                objects[i].transform.lossyScale);

            Vector3 center = collider.center;
            
            Quaternion rotation = objects[i].transform.rotation;

            Vector3[] vertices = new Vector3[9];

            vertices[0] = rotation * new Vector3(center.x - size.x / 2, center.y - size.y / 2, center.z - size.z / 2) + objects[i].transform.position;
            vertices[1] = rotation * new Vector3(center.x + size.x / 2, center.y - size.y / 2, center.z - size.z / 2) + objects[i].transform.position;
            vertices[2] = rotation * new Vector3(center.x - size.x / 2, center.y + size.y / 2, center.z - size.z / 2) + objects[i].transform.position;
            vertices[3] = rotation * new Vector3(center.x + size.x / 2, center.y + size.y / 2, center.z - size.z / 2) + objects[i].transform.position;
            vertices[4] = rotation * new Vector3(center.x - size.x / 2, center.y - size.y / 2, center.z + size.z / 2) + objects[i].transform.position;
            vertices[5] = rotation * new Vector3(center.x + size.x / 2, center.y - size.y / 2, center.z + size.z / 2) + objects[i].transform.position;
            vertices[6] = rotation * new Vector3(center.x - size.x / 2, center.y + size.y / 2, center.z + size.z / 2) + objects[i].transform.position;
            vertices[7] = rotation * new Vector3(center.x + size.x / 2, center.y + size.y / 2, center.z + size.z / 2) + objects[i].transform.position;
            vertices[8] = rotation * collider.bounds.center;
            
            for(int vnum = 0; vnum < 9; vnum++)
            {
                Debug.Log(vnum);
                Debug.Log(vertices[vnum].x);
                Debug.Log(vertices[vnum].y);
                Debug.Log(vertices[vnum].z);
            }
            
            //DrawBound(vertices);

            float points_x = 0;
            float points_y = 0;
            float points_z = 0;
            for (int j = 0; j < 8; j++)
            {
                points_x += vertices[j].x;
                points_y += vertices[j].y;
                points_z += vertices[j].z;
            }
            vertices[8] = new Vector3(points_x/8,points_y/8,points_z/8);

            
            jsonData.objects[i].projected_cuboid = new float[][]
            {
                new float[] { JsonCam.WorldToViewportPoint(vertices[4]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[4]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[5]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[5]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[7]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[7]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[6]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[6]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[0]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[0]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[1]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[1]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[3]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[3]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[2]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[2]).y) * 500f},
                new float[] { JsonCam.WorldToViewportPoint(vertices[8]).x * 500f, (1-JsonCam.WorldToViewportPoint(vertices[8]).y) * 500f }
            };
            //DrawBound(vertices);

           
        }



        #endregion


        //json 파일로 저장
        //string json = JsonUtility.ToJson(camData, true);
        string json = JsonConvert.SerializeObject(jsonData);

        File.WriteAllText(Path.Combine(Jsonpath, indexnumber.ToString() + ".json"), json);

    }

    public void RandomMoveObjects()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.position =
                new Vector3(Random.Range(MinBoundary.x, MaxBoundary.x),
                Random.Range(MinBoundary.y, MaxBoundary.y),
                Random.Range(MinBoundary.z, MaxBoundary.z));

            objects[i].transform.rotation = Quaternion.Euler(new Vector3(
                Random.Range(0f,360f), Random.Range(0f, 360f), Random.Range(0f, 360f)
                ));

        }
    }

    void DrawBound(Vector3[] vertices)
    {
        var newObj = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        newObj.name = "Lines";

        DrawLine(vertices[0], vertices[1], newObj.transform);
        DrawLine(vertices[0], vertices[2], newObj.transform);
        DrawLine(vertices[0], vertices[4], newObj.transform);
        DrawLine(vertices[1], vertices[5], newObj.transform);
        DrawLine(vertices[1], vertices[3], newObj.transform);
        DrawLine(vertices[2], vertices[3], newObj.transform);
        DrawLine(vertices[2], vertices[6], newObj.transform);
        DrawLine(vertices[3], vertices[7], newObj.transform);
        DrawLine(vertices[4], vertices[6], newObj.transform);
        DrawLine(vertices[4], vertices[5], newObj.transform);
        DrawLine(vertices[5], vertices[7], newObj.transform);
        DrawLine(vertices[6], vertices[7], newObj.transform);
    }


    void DrawLine(Vector3 vertices1, Vector3 vertices2, Transform parents)
    {
        for(int i = 0; i <= 100; i++)
        {
            var np = Instantiate(PointObj, ((vertices2 - vertices1) * i) / 100 + vertices1, Quaternion.identity);
            np.transform.SetParent(parents);
            np.name = i.ToString();
        }
    }
}
