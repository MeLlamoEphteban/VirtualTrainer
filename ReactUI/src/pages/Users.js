import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

function Row({item, index, deleteMe}) {
    const navigate = useNavigate();
    const [active, setActive] = useState(false);

    const handleEdit = (id) => {
        navigate("/Users/Edit/" + id);
    }

    function timeout(delay){
        return new Promise( res => setTimeout(res, delay));
    }

    const handleDelete = async (id) => {
        setActive(true);
        await timeout(300);
        const response = window.confirm("Are you sure you want to delete the item?");

        if(response)
        {
            let res = fetch(`http://localhost:5000/api/UsersControllerNew/DeleteUser/${id}`, {
                method: "DELETE"
            }).then ((res) => {
                if(res.status === 200)
                {
                    window.alert("Selection deketed!");
                    deleteMe(item);
                }
            }).catch((e) => {
                console.log("Error in fetch", e);
            })
        }
        else
        {
            setActive(false);
            window.alert("Action aborted!");
        }
    }

    return (
        <tr style={{backgroundColor: active ? "red" : "white"}}>
            <td>{index+1}</td>
            <td>{item.name}</td>
            <td>{item.surname}</td>
            <td>{item.phone}</td>
            <td>{item.email}</td>
            <td>{item.userSubscription.idsubscriptionNavigation.subName}</td>
            <td>{item.userSubscription.endDate}</td>
            <td>
                <button onClick={() => handleEdit(item.iduser)}>Edit</button>
            </td>
            <td>
                <button onClick={() => handleDelete(item.iduser)}>Delete</button>
            </td>
        </tr>
    );
}

function MyComponent() {
    const [data, setData] = useState([]);
    const navigate = useNavigate();

    const handleCreate = () => {
        navigate("/Users/Create");
    }

    const deleteChild = (child) => {
        var x = data.filter(value => value != child);
        setData(x);
    }

    useEffect(() => {
        fetch("http://localhost:5000/api/UsersControllerNew/GetUsersRaw")
        .then(res => res.json())
        .then(
            (result) => {
                setData(result);
            },
            (error) => {
                console.error(error);
            }
        )
}, [])

    return(
        <>
        <h3 onClick={() => handleCreate()}>Create User</h3>
        <table>
        <thead>
            <tr>
            <th></th>
            <th>Name</th>
            <th>Surname</th>
            <th>Phone</th>
            <th>Email</th>
            <th>Sub Name</th>
            <th>Expiry date</th>
            <th>Actions</th>
            </tr>
        </thead>
        <tbody>
        {data.map((item, indexMap) => (
          <Row key={item.iduser} item={item} index={indexMap} deleteMe={deleteChild}/>
        ))}
        </tbody>
        </table>
        </>
        );
    }

export default MyComponent;