using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestNetworkList: NetworkBehaviour
{
    // ^^^^^^^ including all code from previous example ^^^^^^^

    // The weapon booster currently applied to a player
    private NetworkVariable<WeaponBooster> PlayerWeaponBooster = new NetworkVariable<WeaponBooster>();

    /// <summary>
    /// A list of team members active "area weapon boosters" that can be applied if the local player
    /// is within their range.
    /// </summary>
    private NetworkList<AreaWeaponBooster> TeamAreaWeaponBoosters;

    void Awake()
    {
        //NetworkList can't be initialized at declaration time like NetworkVariable. It must be initialized in Awake instead.
        TeamAreaWeaponBoosters = new NetworkList<AreaWeaponBooster>(); 
    }

    void Start()
    {
        /*At this point, the object hasn't been network spawned yet, so you're not allowed to edit network variables! */
        //list.Add(new AreaWeaponBooster());
    }

    void Update()
    {
        //This is just an example that shows how to add an element to the list after its initialization:
        if (!IsServer) { return; } //remember: only the server can edit the list
        if (Input.GetKeyUp(KeyCode.UpArrow)) 
        {
            Debug.Log("Adding a new booster to the list");
            TeamAreaWeaponBoosters.Add(new AreaWeaponBooster());
            Debug.Log($"The list now has {TeamAreaWeaponBoosters.Count} elements");
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsClient)
        {
            TeamAreaWeaponBoosters.OnListChanged += OnClientListChanged;
        }
        if (IsServer)
        {
            TeamAreaWeaponBoosters.OnListChanged += OnServerListChanged;
            TeamAreaWeaponBoosters.Add(new AreaWeaponBooster()); //if you want to initialize the list with some default values, this is a good time to do so.
        }
    }

    void OnServerListChanged(NetworkListEvent<AreaWeaponBooster> changeEvent)
    {
        Debug.Log($"[S] The list changed and now has {TeamAreaWeaponBoosters.Count} elements");
    }

    void OnClientListChanged(NetworkListEvent<AreaWeaponBooster> changeEvent)
    {
        Debug.Log($"[C] The list changed and now has {TeamAreaWeaponBoosters.Count} elements");
    }
}

/// <summary>
/// Example: Complex Type
/// This is an example of how one might handle tracking any weapon booster currently applied
/// to a player. 
/// </summary>
public struct WeaponBooster : INetworkSerializable, System.IEquatable<WeaponBooster>
{
    public float PowerAmplifier;
    public float Duration;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out PowerAmplifier);
            reader.ReadValueSafe(out Duration);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(PowerAmplifier);
            writer.WriteValueSafe(Duration);
        }
    }

    public bool Equals(WeaponBooster other)
    {
        return PowerAmplifier == other.PowerAmplifier && Duration == other.Duration;
    }
}

public struct AreaWeaponBooster : INetworkSerializable, System.IEquatable<AreaWeaponBooster>
{
    public WeaponBooster ApplyWeaponBooster; // the nested complex type
    public float Radius;
    public Vector3 Location;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            // The complex type handles its own de-serialization
            serializer.SerializeValue(ref ApplyWeaponBooster);
            // Now de-serialize the non-complex type properties
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out Radius);
            reader.ReadValueSafe(out Location);
        }
        else
        {
            // The complex type handles its own serialization
            serializer.SerializeValue(ref ApplyWeaponBooster);
            // Now serialize the non-complex type properties
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(Radius);
            writer.WriteValueSafe(Location);
        }
    }

    public bool Equals(AreaWeaponBooster other)
    {
        return other.Equals(this) && Radius == other.Radius && Location == other.Location;
    }
}