import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from 'react-router-dom';

function EditExercise(){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [item, setItem] = useState([]);

    let { exerciseID } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetch(`http://localhost:5000/Exercises/GetExerciseId?id=${exerciseID}`)
        .then(res => res.json())
        .then(
            (result) => {
                setIsLoaded(true);
                setItem(result);
            },
            (error) => {
                setIsLoaded(true);
                setError(error);
        }
        )
    }, [exerciseID])

    const gotoExercises = () => {
      navigate("/Exercises");
    }
    
    if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <div>Loading...</div>;
  } else {
    return (
        <>
        <label>Exercise Name</label>
        <input type="text" id="name" value={item.exerciseName} />
        <label>Exercise Instructions</label>
        <input type="text" id="desc" value={item.instructions} />
        <button onClick={gotoExercises}>Cancel Edit</button>
        </>
    );
  }
}

export default EditExercise;