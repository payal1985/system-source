import { Platform, PermissionsAndroid } from 'react-native'
import Geolocation from '@react-native-community/geolocation'
import AsyncStorage from '@react-native-community/async-storage'
export const getLocation = async () => {
  if (Platform.OS === 'ios') {
    Geolocation.requestAuthorization();
    Geolocation.setRNConfiguration({
      skipPermissionRequests: false,
      authorizationLevel: 'whenInUse',
    });
  }
  if (Platform.OS === 'android') {
    await PermissionsAndroid.request(
      PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION,
    );
  }
  Geolocation.getCurrentPosition(async (currentLocation) => {
    await AsyncStorage.setItem("@custGPS", `${currentLocation.coords.latitude}/${currentLocation.coords.longitude}`)
  })
}