using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities{

    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck){
    
    if(stringToCheck == ""){
        Debug.Log(fieldName + " ta vazio ze, precisa ter algo ai " + thisObject.name.ToString());
        return true;
    }
    return false;

    }

    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck){
        bool error = false;
        int count = 0;

        foreach (var item in enumerableObjectToCheck){
            
            if(item == null){
                Debug.Log(fieldName + " null objects " + thisObject.name.ToString());
                error = true;
            }
            else {
                count++;
            }
        }

        if(count == 0){
            Debug.Log(fieldName + " no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;

    }
}
