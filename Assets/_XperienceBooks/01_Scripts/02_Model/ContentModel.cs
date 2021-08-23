

[System.Serializable]
public class builder_position
{
    public float x;
    public float y;
    public float z;
}


[System.Serializable]
public class builder_rotation
{
    public float x;
    public float y;
    public float z;
}


[System.Serializable]
public class builder_scale
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class ContentModel
{
    public int id;
    public int chapter_id;
    public bool audio_play_in_loop;
    public int c_code_image_id;
    public int ar_module_id;
    public string webpagelink;
    public string c_code_image;
    public string c_code_image_name;
    public string ar_content;
    public string filename;
    public string artist_name;
    public string description;
    public string content_type;
    public string ar_module_name;
    public string chapter_name;
    public string book_name;
    public string author_name;
    public string series_name;
    public builder_position position;
    public builder_rotation rotation;
    public builder_scale scale;
}
