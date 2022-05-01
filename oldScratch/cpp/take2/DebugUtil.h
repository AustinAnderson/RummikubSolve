#ifndef DEBUGUTIL
#define DEBUGUTIL
#include <iostream>
#include <sstream>
#include <vector>
#define ENSURE_DEBUGUTIL_AVAILABLE(T_VectorType)\
template class DebugUtil<T_VectorType>;
//DebugUtil.h: include in source and call the macro with the types
//of vectors to be debugged to allow DebugUtil<T>::debugString(vector<T>)
//to be used in GDB
//Created: Mon Oct  4 23:59:27 CDT 2021

struct DebugString{
    DebugString(){ }
    DebugString(std::string str){
        std::stringstream spliter(str);
        std::string* addrs[16]={
            &str0,&str1,&str2,&str3,&str4,&str5,&str6,&str7,
            &str8,&str9,&strA,&strB,&strC,&strD,&strE,&strF
        };
        int i=0;
        for(std::string each;std::getline(spliter,each);i++){
            *(addrs[i])=each;
        }
    }
    std::string str0;
    std::string str1;
    std::string str2;
    std::string str3;
    std::string str4;
    std::string str5;
    std::string str6;
    std::string str7;
    std::string str8;
    std::string str9;
    std::string strA;
    std::string strB;
    std::string strC;
    std::string strD;
    std::string strE;
    std::string strF;
};
template<bool B,class T>
struct enable_if_or_char{};
template<class T>
struct enable_if_or_char<true,T>{
    typedef T type;
};
template<class T>
struct enable_if_or_char<false,T>{
    typedef char type;
};
template<typename T>
class DebugUtil {
    public:
        
        template<class V=typename enable_if_or_char<std::is_copy_constructible<T>::value,T>::type>
        static DebugString debugString(
                V* list,unsigned int size
        ){
            std::stringstream s;
            s<<"[";
            if(size>0){
                s<<list[0].debugString();
                for(unsigned int i=1;i<size;i++){
                    if(i%20==0) s<<std::endl;
                    s<<","<<list[i].debugString();
                }
            }
            s<<"]";
            return DebugString(s.str());
        }
        template<class V=typename enable_if_or_char<std::is_copy_constructible<T>::value,T>::type>
        static DebugString debugString(
                std::vector<V> list
        ){
            std::stringstream s;
            s<<"[";
            if(list.size()>0){
                s<<list[0].debugString();
                for(unsigned int i=1;i<list.size();i++){
                    if(i%20==0) s<<std::endl;
                    s<<","<<list[i].debugString();
                }
            }
            s<<"]";
            return DebugString(s.str());
        }
        template<class V=typename enable_if_or_char<std::is_copy_constructible<T>::value,T>::type>
        static DebugString debugString(std::vector<V> list,unsigned int startIndex,unsigned int length){
            if(startIndex+length>list.size()){
                std::stringstream err;
                err<<"out of range, startIndex("<<startIndex<<") + length("<<length<<") > list.size("<<list.size()<<")";
                return DebugString(err.str());
            }
            return DebugString(debugString(std::vector<T>(list.data()+startIndex,list.data()+startIndex+length)));
        }


        static std::string strPointer(T* item){
            if(item==nullptr) return "null";
            return item->debugString();
        }
        static DebugString debugString(const std::vector<T*>& list,unsigned int startIndex,unsigned int length){
            if(startIndex+length>list.size()){
                std::stringstream err;
                err<<"out of range, startIndex("<<startIndex<<") + length("<<length<<") > list.size("<<list.size()<<")";
                return DebugString(err.str());
            }
            return DebugString(debugString(std::vector<T*>(list.data()+startIndex,list.data()+startIndex+length)));
        }
        static DebugString debugString(const std::vector<T*>& list){
            std::stringstream s;
            s<<"[";
            if(list.size()>0){
                s<<strPointer(list[0]);
                for(unsigned int i=1;i<list.size();i++){
                    if(i%20==0) s<<std::endl;
                    s<<","<<strPointer(list[i]);
                }
            }
            s<<"]";
            return DebugString(s.str());
        }
};
#endif//DEBUGUTIL
